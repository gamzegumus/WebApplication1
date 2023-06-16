//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using WebApplication1.Models;

//namespace WebApplication1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class New_KullaniciController : ControllerBase
//    {


//        static List<TBL_NEW_KULLANICI> _kullanici = new List<TBL_NEW_KULLANICI>
//        {
//            new TBL_NEW_KULLANICI { ID = 1, Name = "Gamze", Surname = "Gümüş", Email = "gamze@gmail.com", Password = "123456", Password_tekrar = "123456", Username = "gamzeg" },
//            new TBL_NEW_KULLANICI { ID = 2, Name = "Elif", Surname = "Ergen", Email = "elif@gmail.com", Password = "123456", Password_tekrar = "123456", Username = "elife" },
//            new TBL_NEW_KULLANICI { ID = 3, Name = "Ezgi", Surname = "Kaya", Email = "ezgi@gmail.com", Password = "123456", Password_tekrar = "123456", Username = "ezgik" },
//            new TBL_NEW_KULLANICI { ID = 4, Name = "Kübra", Surname = "Gümüş", Email = "kubra@gmail.com", Password = "123456", Password_tekrar = "123456", Username = "kubrag" }

//        };

//        [HttpGet("GetAllUser")]
//        public List<TBL_NEW_KULLANICI> Get()
//        {

//            return _kullanici;
//        }

//        [HttpGet("{id}")]
//        public TBL_NEW_KULLANICI Get(int id)
//        {
//            var kul = _kullanici.FirstOrDefault(x => x.ID == id);
//            return kul;
//        }


//        [HttpPost("AddUser")]
//        public TBL_NEW_KULLANICI Post(TBL_NEW_KULLANICI kul)
//        {
//            _kullanici.Add(kul);
//            return kul;
//        }

//        [HttpPost("New_Kullanici")]
//        public TBL_NEW_KULLANICI Post_login(TBL_NEW_KULLANICI login_kul)
//        {
//            _kullanici.Add(login_kul);
//            return login_kul;
//        }

//        [HttpPut]
//        public TBL_NEW_KULLANICI Put([FromBody]TBL_NEW_KULLANICI kul)
//        {
//            var edit = _kullanici.FirstOrDefault(x => x.ID == kul.ID);
//            edit.Name=kul.Name;
//            edit.Surname=kul.Surname;
//            edit.Username=kul.Username;
//            edit.Password=kul.Password;
//            edit.Email=kul.Email;
//            edit.Password_tekrar=kul.Password_tekrar;
           
//            return kul;
//        }


//        [HttpDelete]

//        public TBL_NEW_KULLANICI Delete(int id)
//        {
//            var data = _kullanici.FirstOrDefault(x => x.ID == id);
//            _kullanici.Remove(data);
//            return data;
//        }
//    }
//}




    
