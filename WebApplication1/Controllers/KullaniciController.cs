//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using WebApplication1.Models;

//namespace WebApplication1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class KullaniciController : ControllerBase
//    {

//        static List<Login_Kullanici> _kullanici = new List<Login_Kullanici>
//        {
//            new Login_Kullanici { ID = 1, Username = "Gamze", Password = "123456" },
//            new Login_Kullanici { ID = 2, Username = "Elif", Password = "123456" },
//            new Login_Kullanici { ID = 3, Username = "Ezgi" , Password = "123456" },
//            new Login_Kullanici { ID = 4, Username = "Kübra", Password = "123456"}

//        };

//        [HttpGet("GetAllUser")]
//        public List<Login_Kullanici> Get()
//        {

//            return _kullanici;
//        }

//        [HttpGet("{id}")]
//        public Login_Kullanici Get(int id)
//        {
//            return _kullanici.FirstOrDefault(x => x.ID == id);
//        }

//        [HttpPost]
//        public Login_Kullanici Post(Login_Kullanici kul)
//        {
//            _kullanici.Add(kul);
//            return kul;
//        }

//        [HttpPost("Login_Kullanici")]
//        public Login_Kullanici Post_login(Login_Kullanici login_kul)
//        {
//            _kullanici.Add(login_kul);
//            return login_kul;
//        }

//        [HttpPost("New_Kullanici")]
//        public Login_Kullanici Post_new(Login_Kullanici _kul)
//        {
//            List<New_Kulanici> itemS = new List<New_Kulanici>();

//            foreach (var item in itemS)
//            {
//                item.ID = item.ID;
//                item.Name = item.Name;
//                item.Surname = item.Surname;
//                item.Username = item.Username;
//                item.Email = item.Email;
//                item.Password_tekrar = item.Password_tekrar;
//                itemS.Add(item);
//            }

//            return _kul;
//        }



//        [HttpDelete("DeleteUser")]

//        public Login_Kullanici Delete(int id)
//        {
//            var data = _kullanici.FirstOrDefault(x => x.ID == id);
//            _kullanici.Remove(data);
//            return data;
//        }


//        [HttpPost("Login_Kullanici")]
//        public Login_Kullanici Post_login_Kul(Login_Kullanici login_kul)
//        {
//            _kullanici.Add(login_kul);
//            return login_kul;
//        }


//    } 
//}





    
