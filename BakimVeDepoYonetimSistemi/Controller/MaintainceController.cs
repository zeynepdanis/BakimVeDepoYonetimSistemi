
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Serialization;

using BakimVeDepoYonetimSistemi.Model;
using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Mvc;

using OfficeOpenXml;

namespace BakimVeDepoYonetimSistemi.Controller
{

    [ApiController]
    public class MaintainceController : ControllerBase
    {

        private readonly MaintenanceRepository _bakimTalepRepository;
        private readonly TeamRepository _teamRepository;
        private readonly UserRepository _userRepository;


        public MaintainceController(MaintenanceRepository bakimTalepRepository, TeamRepository teamRepository, UserRepository userRepository)
        {
            _bakimTalepRepository = bakimTalepRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;

        }

        [HttpPost("create-demand")]
        public IActionResult InsertBakimTalep(MaintainceRequest maintainceRequest)
        {

            var createdDate = DateTime.Now;
            var state = "DevamEdiyor";

            var stateId = _bakimTalepRepository.GetStateId(state);
            var assetId = (int)maintainceRequest.asset.VarlikId;

            var rowsAffected = _bakimTalepRepository.InsertBakimTalep(maintainceRequest.creatorId, createdDate, stateId, assetId, maintainceRequest.demandDescription);

            if (rowsAffected >= 0)
            {


                // E-postayı gönderen methodu çağırma
                SendEmail();


                return Ok(new { message = "created." });

            }
            else
            {
                return BadRequest("Bir hata oluştu.");
            }


        }

        [HttpGet("getallbakimtalep")]
        public IActionResult GetAllBakimTalep()
        {
            var bakimTalep = _bakimTalepRepository.GetAllBakimTalep();

            if (bakimTalep != null)
            {
                return Ok(bakimTalep);
            }
            else
            {
                return BadRequest("Bir hata oluştu.");
            }





        }
        [HttpGet("all-demands")]
        public IActionResult GetAllBakimTalepWithDetails()
        {
            var bakimTalep = _bakimTalepRepository.GetMaintenanceDetails();

            if (bakimTalep != null)
            {
                return Ok(bakimTalep);
            }
            else
            {
                return BadRequest("Bir hata oluştu.");
            }





        }
        [HttpGet("all-demands/export-xml")]
        public IActionResult ExportAllBakimTalepToXml()
        {
            var bakimTalep = _bakimTalepRepository.GetMaintenanceDetails();

            if (bakimTalep != null)
            {
                // XML Serializer kullanarak objeyi XML'e dönüştürme
                var serializer = new XmlSerializer(bakimTalep.GetType());
                var stringWriter = new System.IO.StringWriter();
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, bakimTalep);
                    var xml = stringWriter.ToString();

                    // XML'i dosyaya kaydetmek
                    var filePath = "bakimTalepleri.xml";
                    System.IO.File.WriteAllText(filePath, xml);

                    // Dosya adı ve MIME türünü belirterek dosyayı döndürme
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    System.IO.File.Delete(filePath); // Oluşturulan dosyayı silebilirsiniz

                    return File(fileBytes, "application/xml", "bakimTalepleri.xml");
                }
            }
            else
            {
                return BadRequest("Bir hata oluştu.");
            }
        }
        [HttpGet("all-demands/export-excel")]
        public IActionResult ExportAllBakimTalepToExcel()
        {
            var bakimTalep = _bakimTalepRepository.GetMaintenanceDetails();

            if (bakimTalep != null)
            {


                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("BakimTalepleri");

                    // Başlık satırını ekleyelim
                    worksheet.Cells["A1"].Value = "TalepId";
                    worksheet.Cells["B1"].Value = "Açıklama";
                    worksheet.Cells["C1"].Value = "Kullanıcı Adı Soyadı";
                    worksheet.Cells["D1"].Value = "Varlık Adı";
                    worksheet.Cells["E1"].Value = "Durum";
                    worksheet.Cells["F1"].Value = "Oluşturulma Tarihi";

                    // Diğer sütun başlıklarını da aynı şekilde ekleyebilirsiniz

                    // Verileri Excel'e ekleyelim
                    int row = 2; // Verilerin başlayacağı satır
                    foreach (var talep in bakimTalep)
                    {
                        worksheet.Cells[string.Format("A{0}", row)].Value = talep.TalepId;
                        worksheet.Cells[string.Format("B{0}", row)].Value = talep.Aciklama;
                        worksheet.Cells[string.Format("C{0}", row)].Value = talep.KullaniciAdiSoyadi;
                        worksheet.Cells[string.Format("D{0}", row)].Value = talep.VarlikAdi;
                        worksheet.Cells[string.Format("E{0}", row)].Value = talep.Durum;
                        worksheet.Cells[string.Format("F{0}", row)].Value = talep.OlusturulmaTarihi;


                        // Diğer sütunlardaki verileri de aynı şekilde ekleyin

                        row++;
                    }

                    // İsteğe bağlı: Excel dosyasındaki hücreleri biçimlendirme
                    worksheet.Cells["A1:B1"].Style.Font.Bold = true;
                    worksheet.Cells.AutoFitColumns();

                    // Dosyayı HTTP yanıtı olarak döndürme
                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "bakimTalepleri.xlsx");
                }
            }
            else
            {
                return BadRequest("Bir hata oluştu.");
            }
        }




        [HttpGet("get-maintenance-team-member/{maintenanceTeamType}")]
        public IActionResult GetAllMaintainceMember([FromRoute] string maintenanceTeamType)
        {
            try
            {
                var result = _bakimTalepRepository.GetAllMaintainceMembersByTeamType(maintenanceTeamType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        private void SendEmail()
        {
            // Sender's email credentials
            string senderEmail = "bakimvedepo@gmail.com";
            string senderPassword = "vuag rzlk eish xnqu";

            // Receiver's email
            string receiverEmail = "danisss.zeynep@gmail.com";

            // Create a new MailMessage
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = "Test Email";
            mail.Body = "This is a test email from .NET.";

            // Set up the SMTP client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.Port = 587; // Port for Gmail


            try
            {
                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }



}



