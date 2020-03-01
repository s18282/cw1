using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            foreach(var a in args)
            {
                Console.WriteLine(a);
            }
            
            if (args.Length == 0)
            {
                throw new ArgumentNullException("no arg");
            }

            Uri uriResult;
            bool result = Uri.TryCreate(args[0], UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                throw new ArgumentException("arg not valid");
            } 

            var emails = await GetEmails(args[0]);
            
            foreach(var email in emails)
            {
                Console.WriteLine(email);
            }
        }

        static async Task<IList<string>> GetEmails(string url)
        {
            var httpClient = new HttpClient();
            var listOfEmails = new List<string>();
            var response = await httpClient.GetAsync(url);
            httpClient.Dispose();
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            string content = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            
            MatchCollection emailMatches = emailRegex.Matches(content);
            
            foreach (var emailMatch in emailMatches)
            {
                listOfEmails.Add(emailMatch.ToString());
            }
            return listOfEmails;
        }
    }
}
