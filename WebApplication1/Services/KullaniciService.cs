//using WebApplication1.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Microsoft.AspNetCore.Identity;

//namespace WebApplication1.Services
//{
//    public class KullaniciService : IKullaniciService
//    {
//        private string uri = "https://localhost:7148";
      

//        public async Task<string> Login_Kullanici(Login_Kullanici login_kullanici)
//        {
//            string returnStr = string.Empty;
//            using (var http = new HttpClient())
//            {
//                var url = $"{uri}{API.Login_Kullanici}";
//                var str = JsonConvert.SerializeObject(login_kullanici);
//                var response = await http.PostAsync(url, new StringContent(str, Encoding.UTF8, "application/json"));

//                if (response.IsSuccessStatusCode)
//                {
//                    returnStr = await response.Content.ReadAsStringAsync();
//                }
//            }
//            return returnStr;
//        }

//        public async Task<(bool IsSuccess, string ErrorMessage)> New_Kullanici(New_Kulanici new_kullanici)
//        {
//            string errorMessage = string.Empty;
//            bool isSuccess = false;
//            using (var http = new HttpClient())
//            {
//                var url = $"{uri}{API.New_Kullanici}";
//                var str = JsonConvert.SerializeObject(new_kullanici);
//                var response = await http.PostAsync(url, new StringContent(str, Encoding.UTF8, "application/json"));

//                if (response.IsSuccessStatusCode)
//                {
//                   isSuccess = true;
//                }
//                else
//                {
//                    errorMessage = await response.Content.ReadAsStringAsync();
//                }
//            }
//            return (isSuccess, errorMessage);
//        }



//    }
//}
        
    

