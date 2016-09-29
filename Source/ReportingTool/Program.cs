using System;
using System.Collections.Generic;
using System.Text;

using AdobeConnectSDK;
using AdobeConnectSDK.Model;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Threading;

namespace ReportingTool
{
  class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            Console.WriteLine();
            Console.WriteLine("AC reporting tool, build " + Assembly.GetExecutingAssembly().GetName().Version.ToString());            
            Console.WriteLine();

            if (args.Length < 1)
            {
                Console.WriteLine("Params:");
                Console.WriteLine("-o= CalcQuizBelts | ListQuizes | ListWebinars | QuizReports | WebinarReports | SurveyResponses | ListPrincipals | ListPrincipalsByGroup");
                Console.WriteLine("-id= room sco id | quiz sco id ");
                Console.WriteLine("-topic= topic Name");
                Environment.Exit(-1);
            }

            CmdLineParams cmdParams = new CmdLineParams(args);

            if (cmdParams["o"] == null)
            {
                Console.WriteLine("operation Name should be specified.");
                Environment.Exit(-1);
            }

            AdobeConnectXmlAPI acConn = _acConnect();

            Console.WriteLine("Processing...");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            try
            {                
                switch (cmdParams["o"])
                {
                    case "ListQuizes":
                        CheckScoSupplied(cmdParams);
                        //new acDataManager(acConn).ListQuizes(cmdParams["id"]);
                        break;
                    case "ListWebinars":
                        CheckScoSupplied(cmdParams);
                        //new acDataManager(acConn).ListWebinars(cmdParams["id"]);
                        break;
                    case "QuizReports":
                        CheckScoSupplied(cmdParams);
                        //new acDataManager(acConn).GetQuizReports(cmdParams["id"]);
                        break;
                    case "WebinarReports":
                        CheckScoSupplied(cmdParams);
                        //new acDataManager(acConn).GetWebinarReports(cmdParams["id"]);
                        break;
                    case "SurveyResponses":
                        CheckScoSupplied(cmdParams);
                        //new acDataManager(acConn).GetSurveyResponses(cmdParams["id"]);
                        break;
                    case "CreateTestPrincipal":
                        //acConn.PrincipalUpdate(new PrincipalSetup { Login = "dm_tst", Name = "dm_tst", FirstName = "dm_tst", LastName = "dm_tst", password = "dm_tst", Type = PrincipalTypes.external_user });
                        //acConn.PrincipalUpdate(new PrincipalSetup { Login = "dm_tst", Name = "dm_tst",  Password = "dm_tst", PrincipalType = PrincipalTypes.user }, out );
                        break;
                    case "ListPrincipals":
                        //new acDataManager(acConn).ListPrincipals();
                        break;
                    case "ListPrincipalsByGroup":
                        //new acDataManager(acConn).ListPrincipals(cmdParams["id"]);
                        break;
                    case "ReportQuotas":
                        //new acDataManager(acConn).ReportQuotas();
                        break;

                  default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                watch.Stop();
                Console.WriteLine("Time elapsed: " + watch.Elapsed.ToString());
                acConn.Logout();
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static AdobeConnectXmlAPI _acConnect()
        {
            AdobeConnectXmlAPI acConn = new AdobeConnectXmlAPI();
            ApiStatus s;
            //if (!acConn.Login(ConfigurationManager.AppSettings["ACxmlAPI_netUser"], ConfigurationManager.AppSettings["ACxmlAPI_netPassword"], out sInfo))
            {
                Console.WriteLine("Unable to logon. Please check cfg.");
                Environment.Exit(-1);
            }
            return acConn;
        }

        private static void CheckScoSupplied(CmdLineParams cmdParams)
        {
            if (cmdParams["id"] == null)
            {
                Console.WriteLine("sco id (-id) should be specified.");
                Environment.Exit(-1);
            }
        }



    }
}
