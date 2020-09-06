using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace RevisionWriter
{
    /// <summary>
    /// ScheissDenWirBrauchenKlasse contains all properties needed to write a azubi revision. 
    /// Three of which are obligatory properties(Besamung, Description, Date) 
    /// and three optional (User, ProjectID, Duration).
    /// </summary>
    public class WorkWeek
    {
        public WorkWeek()
        {
            Monday = new ObservableCollection<SingleTask>();
            Tuesday = new ObservableCollection<SingleTask>();
            Wednesday = new ObservableCollection<SingleTask>();
            Thursday = new ObservableCollection<SingleTask>();
            Friday = new ObservableCollection<SingleTask>();
         
            WeekDayArray = new ObservableCollection<SingleTask>[5];
            
            
        }
        public ObservableCollection<SingleTask>[] WeekDayArray = new ObservableCollection<SingleTask>[5];



        public ObservableCollection<SingleTask> Monday = new ObservableCollection<SingleTask>();
        public ObservableCollection<SingleTask> Tuesday = new ObservableCollection<SingleTask>();
        public ObservableCollection<SingleTask> Wednesday = new ObservableCollection<SingleTask>();
        public ObservableCollection<SingleTask> Thursday = new ObservableCollection<SingleTask>();
        public ObservableCollection<SingleTask> Friday = new ObservableCollection<SingleTask>();

        public class SingleTask
        {
            public DateTime Date { get; set; }
            public string User { get; set; }
            public string Description { get; set; }
            public string Besamung { get; set; }
            public string ProjectID { get; set; }
            public string Duration { get; set; }
        }

        /// <summary>
        /// Method to extract all ScheissDenWirBrauchenKlasse properties from odoo html file into collection of ScheissDenWirBrauchenKlasse.
        /// Takes regex and html file as parameter. Returns ObservableCollection<ScheissDenWirBrauchenKlasse>
        /// </summary>
        /// <param name="reg">Regex to get text in between html tags</param>
        /// <param name="html">Odoo html file</param>
        /// <returns>ObservableCollection<ScheissDenWirBrauchenKlasse></returns>
        public ObservableCollection<SingleTask> posoFactory(Regex reg, string html)
        {
            MatchCollection matches = reg.Matches(html);
            int DailyTasksCount = matches.Count;

            ObservableCollection<SingleTask> TasksTotal = new ObservableCollection<SingleTask>();
            for (int i = 0; i < DailyTasksCount; i++)
            {
                SingleTask singleTask = new SingleTask();
                singleTask.Date = Convert.ToDateTime(matches[i].ToString());
                singleTask.User = matches[i + 1].ToString();
                singleTask.Besamung = matches[i + 2].ToString();
                singleTask.ProjectID = matches[i + 3].ToString();
                singleTask.Description = matches[i + 4].ToString();

                i += 4;

                TasksTotal.Add(singleTask);
            }
            return TasksTotal;
        }

          

        public WorkWeek SortTasksByDay (DateTime startDate, ObservableCollection<SingleTask> tasksTotal)
        {
            WorkWeek singleWeek = new WorkWeek();
            singleWeek.WeekDayArray[0] = Monday;
            singleWeek.WeekDayArray[1] = Tuesday;
            singleWeek.WeekDayArray[2] = Wednesday;
            singleWeek.WeekDayArray[3] = Thursday;
            singleWeek.WeekDayArray[4] = Friday;
 
            var foo = tasksTotal.OrderBy(x => x.Date);
            int i = 0;
            while (i < 6)
            {
                foreach (SingleTask task in foo)
                {
                    if (task.Date.Equals(startDate))
                    {
                        singleWeek.WeekDayArray[i].Add(task);
                    }
                }
                i++;
                startDate = startDate.AddDays(1);
            }
            return singleWeek;
        }
    }
}

