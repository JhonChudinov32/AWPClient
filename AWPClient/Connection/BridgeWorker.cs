using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Text;
using AWPClient.Classes;
using AWPClient.LogServices;
using AWPClient.ViewModels;

namespace AWPClient.Connection
{
    public class BridgeWorker : IBridgeWorker
    {

        private string awp_screens = string.Empty;
        private string awp_datasets = string.Empty;
        private string awp_variables = string.Empty;
        private string awp_errors = string.Empty;
        private string awp_grid = string.Empty;
        private string awp_url = MainWindowViewModel.Url;
        private string project = MainWindowViewModel.Project;
        private string environment = MainWindowViewModel.Environment;

        /// <summary>
        ///  Парсим данные глобальной переменной AwpApi
        /// </summary>
        public void ParseParamBridgeWorker()
        {
            foreach (string vars in MainWindowViewModel.AwpApi)
            {
                string[] s = null;
                s = vars.Split(' ');

                if (s != null && s.Length == 2)
                {

                    if (s[0] == "screens")
                    {
                        awp_screens = s[1];
                    }
                    if (s[0] == "variables")
                    {
                        awp_variables = s[1];
                    }
                    if (s[0] == "datasets")
                    {
                        awp_datasets = s[1];
                    }
                    if (s[0] == "errors")
                    {
                        awp_errors = s[1];
                    }
                    if (s[0] == "grid")
                    {
                        awp_grid = s[1];
                    }
                }
            }
        }

        /// <summary>
        ///Конвертируем JSON в DataSet
        /// <summary>
        private DataSet MakeQuery(string jsonResult)
        {
            
            DataSet ds = new DataSet();
            try
            {

                // Преобразование JSON в объект JArray
                JArray jsonArray = JArray.Parse(jsonResult);
               
                // Создание DataTable
                DataTable dataTable = new DataTable();

                // Добавление колонок в DataTable
                foreach (JProperty property in jsonArray.First.Children<JProperty>())
                {
           
                    dataTable.Columns.Add(property.Name);
                }

                // Добавление данных в DataTable
                foreach (JObject jsonObject in jsonArray.Children<JObject>())
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (JProperty property in jsonObject.Properties())
                    {
                        dataRow[property.Name] = property.Value;
                        
                    }
                    dataTable.Rows.Add(dataRow);
                }

                // Добавление DataTable в DataSet
                ds.Tables.Add(dataTable);
                

            }
            catch (Exception ex)
            {
                MainWindowViewModel._message = ex.Message;
            }
            return ds;
        }

        /// <summary>
        /// Выполняет процедуру и возвращает результат запроса в виде списка пар "ключ-значение"
        /// </summary>
        public string ExecProc(string proc, out List<KeyValuePair<string, string>> res)
        {
     
            JArray json_screen;
            res = new List<KeyValuePair<string, string>>();
            try
            {
                DataSet ds = new DataSet();
                BridgeWorkerService httpClientService = new BridgeWorkerService();

                ParseParamBridgeWorker();

                string baseUrl = awp_url + project + "/" + environment + awp_screens;

                string url = $"{baseUrl}/{proc}";

                string jsonResult = httpClientService.GetJsonData(url).TrimStart();

                dynamic data = JsonConvert.DeserializeObject(jsonResult);

                string json = data.data.ToString();

                json_screen = JArray.Parse(json);

                foreach (JObject obj in json_screen.Children<JObject>())
                {
                    foreach (JProperty p in obj.Properties())
                    {
                        res.Add(new KeyValuePair<string, string>(p.Name, p.Value.ToString()));

                    }
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя процедуры: " + proc });
                // Возвращаем строку с ошибкой
                return $"Error: {ex.Message}";
            }
            return string.Empty;
        }

        /// <summary>
        /// Вызывает wpf_SetVariable с указанными параметрами
        /// </summary>
        public bool SetVariable(string nameStr, string valueStr)
        {
            try
            {
                BridgeWorkerService httpClientService = new BridgeWorkerService();
                ParseParamBridgeWorker();

                string baseUrl = awp_url + project + "/" + environment + awp_variables;

                var data = new
                {
                    name = nameStr,
                    value = valueStr
                };

                // Сериализуйте объект в JSON
                var json = JsonConvert.SerializeObject(data);

                // Создайте StringContent с JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправьте POST-запрос
                var response = httpClientService.PostJsonData(baseUrl, content);



                return true;
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя переменной: " + nameStr, "значение переменной: " + valueStr });
                return false;
            }
        }

        /// <summary>
        /// точка для отправки sql запросов
        /// </summary>
        public bool SetGridProc(string sql)
        {
            try
            {
                BridgeWorkerService httpClientService = new BridgeWorkerService();
                ParseParamBridgeWorker();

                string baseUrl = awp_url + project + "/" + environment + awp_grid;

                var data = new
                {
                    query_proc = sql
                };

                // Сериализуйте объект в JSON
                var json = JsonConvert.SerializeObject(data);

                // Создайте StringContent с JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправьте POST-запрос
                var response = httpClientService.PostJsonData(baseUrl, content);

                return true;
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя переменной: " + sql });
                return false;
            }
        }

        /// <summary>
        /// Выполняет процедуру и возвращает результат запроса в виде DataSet
        /// </summary>
        public DataSet SQLQuery(string sql)
        {
            //string filePath = "result.txt";
            //File.WriteAllText(filePath, sql);
            DataSet ds = new DataSet();

            try
            {
                BridgeWorkerService httpClientService = new BridgeWorkerService();
                ParseParamBridgeWorker();

                //string pattern = @"exec\s(.+)\s@\S*";
                //Match match = Regex.Match(sql, pattern);

                string result = string.Empty;
                result = sql;

                //if (match.Success)
                //{
                //    result = "dbo." + match.Groups[1].Value; // Получаем значение после "exec" и до "@"

                //}
                //else
                //{
                //    result = sql;
                //}

                string baseUrl = awp_url + project + "/" + environment + awp_datasets;

                string url = $"{baseUrl}/{result}";

                string jsonResult = httpClientService.GetJsonData(url);
              
                dynamic data = JsonConvert.DeserializeObject(jsonResult);

                ds = MakeQuery(data.data.ToString());
                
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "procedure_name: " + sql });
            }

            return ds;
        }

        /// <summary>
        /// Записывает результат ошибок в БД
        /// </summary>
        public bool SetError(string proc, string messageStr, string descriptionStr, string[] dataStr)
        {
            try
            {
                BridgeWorkerService httpClientService = new BridgeWorkerService();

                ParseParamBridgeWorker();

                string baseUrl = awp_url + project + "/" + environment + awp_errors;

                string dataString = string.Empty;

                if (dataStr != null)
                {
                    foreach (string item in dataStr)
                    {
                        string str = (item != null) ? item.Replace("'", "\"") : string.Empty;
                        dataString += str + ", ";
                    }
                    if (dataString != string.Empty)
                    {
                        dataString = dataString.Remove(dataString.Length - 2);
                    }
                }
                messageStr = (messageStr != null) ? messageStr.Replace("'", "\"") : string.Empty;
                descriptionStr = (descriptionStr != null) ? descriptionStr.Replace("'", "\"") : string.Empty;

                var datа = new
                {
                    log_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                    procedure_name = proc,
                    message = messageStr,
                    description = descriptionStr,
                    data = dataString
                };

                // Сериализуйте объект в JSON
                var json = JsonConvert.SerializeObject(datа);

                // Создайте StringContent с JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправьте POST-запрос
                var response = httpClientService.PostJsonData(baseUrl, content);

                return true;
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "procedure_name: " + proc });
                return false;
            }
        }

    }
}

