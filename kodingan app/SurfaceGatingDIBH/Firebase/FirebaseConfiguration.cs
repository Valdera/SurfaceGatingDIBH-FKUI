using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurfaceGatingDIBH {
    public static class FirebaseConfiguration {

        public static IFirebaseClient client;

        public static string AuthSecretCode { get; set; } = "3hlRhRfbJid4AJluwZWaa1UruYbWG8VwTblBhQWh";

        public static string BasePathURL { get; set; } = "https://surfacegatingdibh.firebaseio.com/";

        public static IFirebaseConfig config = new FirebaseConfig {
            AuthSecret = AuthSecretCode ,
            BasePath = BasePathURL
        };

        public static void UpdateConfig() {
            config = new FirebaseConfig {
                AuthSecret = AuthSecretCode,
                BasePath = BasePathURL
            };
        }

        public static bool CheckData(Patient patient) {
            if(patient.Name == "" || patient.Gender == "" || patient.Id == "" || patient.Age == 0) {
                return false;
            } else {
                return true;
            }
            
        }
    }

}
