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

namespace ReportingTool
{
  using System.Linq;

  class acDataManager
    {
        AdobeConnectXmlAPI m_acConn = null;
        StatusInfo sInfo;

        public acDataManager(AdobeConnectXmlAPI acConn)
        {
            m_acConn = acConn;
        }

        public void GetSurveyResponses(string SurveyID)
        {
            //MeetingDetail _surveyDetail = m_acConn.GetMeetingDetail(SurveyID);

            /*
            XmlNodeList _surveyIteractions = m_acConn.Report_QuizInteractions(SurveyID, string.Empty, out sInfo);
            XmlNodeList _surveyQuestionDistribution = m_acConn.Report_QuizQuestionDistribution(SurveyID, string.Empty, out sInfo);
            if (_surveyIteractions == null || _surveyQuestionDistribution == null) return;

            DataTable _TblSurveyIteractions = GetDataTable(_surveyIteractions);
            DataTable _TblSurveyQuestionDistribution = GetDataTable(_surveyQuestionDistribution);
            _TblSurveyQuestionDistribution.Columns.Add(new DataColumn("QuestionName"));

            foreach (DataRow qaRow in _TblSurveyQuestionDistribution.Rows)
            {
                DataRow[] dr = _TblSurveyIteractions.Select("[interaction-id]=" + qaRow["interaction-id"].ToString());
                if (dr == null || dr.Length < 1) continue;

                qaRow["QuestionName"] = dr[0]["Description"];
            }
            _TblSurveyQuestionDistribution.AcceptChanges();
            */

            XmlNodeList _surveyQuestionDistribution = m_acConn.Report_QuizQuestionDistribution(SurveyID, string.Empty, out sInfo);
            if (_surveyQuestionDistribution == null) return;
            DataTable _TblSurveyQuestionDistribution = GetDataTable(_surveyQuestionDistribution);

            DumpResultsCSV(_TblSurveyQuestionDistribution, "Report_SurveyResponses", string.Empty);

            //XmlNodeList _surveyIteractions = m_acConn.Report_BulkQuestions(string.Empty, out sInfo);
            //DumpResultsCSV(GetDataTable(_surveyIteractions), "Report_BulkQuestions", string.Empty);

            return;
            
            /*
            XmlNodeList _surveyData = m_acConn.Report_QuizQuestionResponse(SurveyID, string.Empty, out sInfo);

            if(_surveyData!=null)
                foreach (XmlNode node in _surveyData)
                {
                    
                }
            */
        }

        public void ListQuizes(string sco_id)
        {
          throw new NotImplementedException();

          /*
          var operationStatus = m_acConn.GetQuizzesInRoom(sco_id);
          DataTable eventsData = GetDataTable(operationStatus.ResultDocument);
            if (eventsData != null)
                DumpResultsCSV(eventsData, "Quizzes_" + sco_id, "url-path ASC");
            else Console.WriteLine("ListQuizes: no data available for given sco-id.");
           */
        }

        public void ListWebinars(string sco_id)
        {
            DataTable eventsData = GetDataTable(m_acConn.GetMeetingsInRoomRaw(sco_id, out sInfo));
            if (eventsData != null)
                DumpResultsCSV(eventsData, "Webinars_" + sco_id, "url-path ASC");
            else Console.WriteLine("ListQuizes: no data available for given sco-id.");
        }

        public void GetQuizReports(string sco_id)
        {
            //NOTE: transactions are not available for quizzes (ref. to quizreport viewer)
            /*
            bool isQuiz = true;
            if (!isQuiz)
            {
                string s_login = "928";
                XmlNodeList rNodes = m_acConn.Report_QuizTakers(ScoId, string.Empty, out sInfo);
                //TransactionInfo[] tInfo = acConn.Report_ConsolidatedTransactions("filter-gt-date-created=2008-01-01", out sInfo);
                DateTime refDate = DateTime.UtcNow.AddMonths(-2);
                string dateFrom = string.Format("{0}-{1}-01", refDate.Year, refDate.Month);
                TransactionInfo[] tInfo = m_acConn.Report_ConsolidatedTransactions(string.Format("filter-Login={0}&filter-gt-date-created={1}", s_login, dateFrom), out sInfo);

                ReportHelper report = new ReportHelper(tInfo, m_acConn);
                ReportHelper.QuizResult[] qResults = m_acConn.GetQuizResultsByAttendee(s_login);
                DumpResultsCSV(qResults, s_login);
            }
            else
            */
            {
                //rNodes: 
                //transcript-id sco-id principal-id status score max-score asset-id permission-id attempts time-taken certificate answered-survey version   
                //1. Report_QuizTakers => transcript-id, sco-id, principal-id, Name, principal_name, Login, date_created
                XmlNodeList rNodes = m_acConn.Report_QuizTakers(sco_id, string.Empty, out sInfo);
                DataTable qTakers = GetDataTable(rNodes);
                rNodes = null;

                //2. QuizIteractions => transcript-id interaction-id sco-id, question Name
                rNodes = m_acConn.Report_QuizInteractions(sco_id, string.Empty, out sInfo);
                DataTable qInteractions = GetDataTable(rNodes);
                rNodes = null;

                //3. QuizQuestionDistribution => interaction-id num-correct num-incorrect total-responses percentage-correct score 
                rNodes = m_acConn.Report_QuizQuestionDistribution(sco_id, string.Empty, out sInfo);
                DataTable qQuestionDistribution = GetDataTable(rNodes);
                rNodes = null;

                //out: ScoId, PrincipalID, num-correct num-incorrect total-responses percentage-correct score

                if (qTakers != null)
                    DumpResultsCSV(qTakers, "QuizTakers_" + sco_id, null);
                else Console.WriteLine("QuizTakers: no data available.");
                if (qInteractions != null)
                    DumpResultsCSV(qInteractions, "qInteractions_" + sco_id, null);
                else Console.WriteLine("qInteractions: no data available.");
                if (qQuestionDistribution != null)
                    DumpResultsCSV(qQuestionDistribution, "qQuestionDistribution_" + sco_id, null);
                else Console.WriteLine("qQuestionDistribution: no data available.");

            }
        }

        public void GetWebinarReports(string sco_id)
        {
            XmlNodeList rNodes = m_acConn.Report_MeetingAttendance(sco_id, string.Empty, out sInfo);
            DataTable MeetingAttendance = GetDataTable(rNodes);
            rNodes = null;

            if (MeetingAttendance != null)
                DumpResultsCSV(MeetingAttendance, "MeetingAttendance_" + sco_id, null);
            else Console.WriteLine("MeetingAttendance: no data available for given sco-id.");

        }


        public void ListPrincipals()
        {
          var pList = m_acConn.GetPrincipalList(string.Empty, string.Empty);

          DataTable PrincipalList = GetDataTable(pList.ToArray());

          if (PrincipalList != null)
            DumpResultsCSV(PrincipalList, "PrincipalList", null);
          else
            Console.WriteLine("PrincipalList: no data available for given group-id.");
        }

        public void ListPrincipals(string GroupID)
        {
            var pList = m_acConn.GetPrincipalList(GroupID, string.Empty);

            DataTable PrincipalList = GetDataTable(pList.ToArray());

            if (PrincipalList != null)
                DumpResultsCSV(PrincipalList, "PrincipalList", null);
            else Console.WriteLine("PrincipalList: no data available for given group-id.");
        }

        public void ReportQuotas()
        {
          m_acConn.Report_Quotas(out sInfo);
        }

        #region internals        
        
        private DataTable GetDataTable(object[] ObjRef)
        {
            if (ObjRef == null) return null;
            DataTable Tbl = new DataTable();

            foreach (FieldInfo fi in ObjRef[0].GetType().GetFields())
            {
                if (!fi.IsPublic) continue;

                if (!Tbl.Columns.Contains(fi.Name))
                    Tbl.Columns.Add(new DataColumn(fi.Name));
            }

            foreach (object obj in ObjRef)
            {
                DataRow dr = Tbl.NewRow();

                foreach (FieldInfo fi in obj.GetType().GetFields())
                {
                    if (!fi.IsPublic) continue;

                    object _val = fi.GetValue(obj);

                    if (_val == null)
                    {
                        dr[fi.Name] = null;
                        continue;
                    }

                    if (_val.GetType().Equals(typeof(DateTime)) && _val.Equals(DateTime.MinValue))
                    {
                        dr[fi.Name] = null;
                    }
                    else
                        dr[fi.Name] = _val;
                }

                Tbl.Rows.Add(dr);
            }

            Tbl.AcceptChanges();

            return Tbl;
        }

        private DataTable GetDataTable(XmlNodeList rNodes)
        {
            if (rNodes == null) return null;

            DataTable Tbl = new DataTable();

            foreach (XmlNode node in rNodes)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (!Tbl.Columns.Contains(attr.Name))
                        Tbl.Columns.Add(new DataColumn(attr.Name));
                }

                foreach (XmlNode iNode in node.ChildNodes)
                {
                    if (!Tbl.Columns.Contains(iNode.Name))
                        Tbl.Columns.Add(new DataColumn(iNode.Name));
                }
            }

            foreach (XmlNode node in rNodes)
            {
                DataRow dr = Tbl.NewRow();

                foreach (XmlAttribute attr in node.Attributes)
                {
                    dr[attr.Name] = attr.Value;
                }

                foreach (XmlNode iNode in node.ChildNodes)
                {
                    dr[iNode.Name] = iNode.InnerText;
                }

                Tbl.Rows.Add(dr);
            }
            Tbl.AcceptChanges();

            return Tbl;
        }

        public static void DumpResultsCSV(DataTable qResults, string repName, string sortCondition)
        {
            StringBuilder sb = new StringBuilder();

            List<string> lstColumns = new List<string>();

            foreach (DataColumn col in qResults.Columns)
            {
                lstColumns.Add(col.Caption);
            }
            sb.AppendLine(string.Join(";", lstColumns.ToArray()));

            DataRow[] dataRows = (!string.IsNullOrEmpty(sortCondition)) ? qResults.Select("", sortCondition) : qResults.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows);

            foreach (DataRow result in dataRows)
            {
                List<string> lstData = new List<string>();

                foreach (DataColumn col in qResults.Columns)
                {
                    if (result[col] != DBNull.Value)
                    {
                        lstData.Add(result[col].ToString());
                    }
                    else
                    {
                        lstData.Add("N/A");
                    }
                }

                sb.AppendLine(string.Join(";", lstData.ToArray()));
            }

            File.WriteAllText(string.Format("rep_{0}_{1}.csv", DateTime.UtcNow.ToString("MMyyyy"), repName), sb.ToString());
        }

        private void DumpResultsCSV(ReportHelper.QuizResult[] qResults, string repName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("quiz id;quiz Name;Login;status;score;time taken (minutes)");

            foreach (ReportHelper.QuizResult result in qResults)
            {
                if (string.IsNullOrEmpty(result.status))
                {
                    result.status = "N/A";
                }

                sb.AppendFormat("{0};{1};{2};{3};{4};{5}", result.sco_id, result.QuizName, result.login, result.status, result.score, result.time_taken / 60);
                sb.AppendLine();
            }

            File.WriteAllText(string.Format("rep_{0}_{1}.csv", DateTime.UtcNow.ToString("MMyyyy"), repName), sb.ToString());
        }

        #endregion
    }
}
