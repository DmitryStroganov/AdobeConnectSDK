using System;
using System.Collections.Generic;
using System.Text;
using AdobeConnectSDK;
using System.Xml;
using System.Diagnostics;
using System.Globalization;
using AdobeConnectSDK.Model;

namespace ReportingTool
{  
  public class ReportHelper
    {
        List<TransactionInfo> tInfoColl = null;
        AdobeConnectXmlAPI acConn = null;

        public ReportHelper(TransactionInfo[] tInfo, AdobeConnectXmlAPI acConnRef)
        {
            if (tInfo == null) throw new ArgumentNullException("Transaction info can't be null");

            tInfoColl = new List<TransactionInfo>();
            tInfoColl.AddRange(tInfo);

            acConn = acConnRef;
        }

        public List<TransactionInfo> GetTransactionsByAttendee(string AttendeeEmail)
        {
           return tInfoColl.FindAll(delegate(TransactionInfo tInfo)
           {
               return tInfo.Login.Equals(AttendeeEmail);
           });
        }

        public string[] GetQuizzesByAttendee(string AttendeeEmail)
        {
            List<TransactionInfo> attendeeOperations = GetTransactionsByAttendee(AttendeeEmail);

            attendeeOperations.Sort(
                new Comparison<TransactionInfo>(delegate(TransactionInfo t1, TransactionInfo t2)
                {
                    return t1.ScoID.CompareTo(t2.ScoID);
                }));

            string scoBuf = null;
            attendeeOperations.RemoveAll(delegate(TransactionInfo tInfo)
            {
                bool flag = false;

                if (scoBuf != null)
                    flag = tInfo.ScoID.Equals(scoBuf);
                scoBuf = tInfo.ScoID;

                return flag;
            });

            return attendeeOperations.ConvertAll(new Converter<TransactionInfo, string>(delegate(TransactionInfo tInfo)
            {
                return tInfo.ScoID;
            })).ToArray();
        }

        public string[] GetQuizzesByAttendee(string AttendeeEmail, DateTime startingDate)
        {
            List<TransactionInfo> attendeeOperations = GetTransactionsByAttendee(AttendeeEmail);

            attendeeOperations.Sort(
                new Comparison<TransactionInfo>(delegate(TransactionInfo t1, TransactionInfo t2)
                {
                    return t1.ScoID.CompareTo(t2.ScoID);
                }));

            string scoBuf = null;
            attendeeOperations.RemoveAll(delegate(TransactionInfo tInfo)
            {
                bool flag = false;

                if (scoBuf != null)
                    flag = tInfo.ScoID.Equals(scoBuf);
                scoBuf = tInfo.ScoID;

                return flag;
            });

            return attendeeOperations.ConvertAll(new Converter<TransactionInfo, string>(delegate(TransactionInfo tInfo)
            {
                return tInfo.ScoID;
            })).ToArray();
        }

        public QuizResult[] GetQuizResultsByAttendee(string AttendeeEmail)
        {
            if (acConn == null) return null;

            List<QuizResult> qResults = new List<QuizResult>();

            string[] quizzes = GetQuizzesByAttendee(AttendeeEmail);

            StatusInfo sInfo;

            foreach (string qID in quizzes)
            {
                XmlNodeList rNodes = acConn.Report_QuizTakers(qID, string.Empty, out sInfo);
                foreach (XmlNode node in rNodes)
                {
                    //retrieve results for given account info
                    if (node.SelectSingleNode("Login/text()").Value.Equals(AttendeeEmail))
                    {
                        try
                        {
                            QuizResult result = new QuizResult();
                            result.sco_id = node.Attributes["sco-id"].Value;
                            result.asset_id = node.Attributes["asset-id"].Value;
                            int.TryParse(node.Attributes["attempts"].Value, out result.attempts);
                            int.TryParse(node.Attributes["time-taken"].Value, out result.time_taken);

                            if (!DateTime.TryParseExact(node.SelectSingleNode("date-created/text()").Value, @"yyyy-MM-dd\THH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal, out result.date_created))
                                result.date_created = default(DateTime);

                            result.login = node.SelectSingleNode("Login/text()").Value;
                            result.principal_name = node.SelectSingleNode("principal-Name/text()").Value;

                            if (node.SelectSingleNode("status/text()") != null)
                            {
                                result.status = node.SelectSingleNode("status/text()").Value;
                            }
                            result.QuizName = node.SelectSingleNode("Name/text()").Value;                            

                            
                            qResults.Add(result);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                        }
                    }
                    
                }
            }


            return qResults.ToArray();
        }


        //-------------------------

        public class QuizResult
        {   
            public string sco_id;
            public string principal_id;
            public string status;
            public int score;
            //for quiz summary reference
            public string asset_id;
            public int attempts;
            public int time_taken;
            public string QuizName;
            public string login;
            public DateTime date_created;
            public string principal_name;
        }
    }
}
