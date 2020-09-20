using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using RevisionWriter;

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
            WorkDay = new ObservableCollection<Task>[5];
        }
        public ObservableCollection<Task>[] WorkDay = new ObservableCollection<Task>[5];

        private enum Weekdays
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
        }
        /// <summary>
        /// Method to extract all ScheissDenWirBrauchenKlasse properties from odoo html file into collection of ScheissDenWirBrauchenKlasse.
        /// Takes regex and html file as parameter. Returns ObservableCollection<ScheissDenWirBrauchenKlasse>
        /// </summary>
        /// <param name="reg">Regex to get text in between html tags</param>
        /// <param name="html">Odoo html file</param>
        /// <returns>ObservableCollection<ScheissDenWirBrauchenKlasse></returns>
        public ObservableCollection<Task> makeTasksFromHtml(Regex reg, string html)
        {
            MatchCollection matches = reg.Matches(html);
            int DailyTasksCount = matches.Count;

            ObservableCollection<Task> TasksTotal = new ObservableCollection<Task>();
            for (int i = 0; i < DailyTasksCount; i++)
            {
                Task Task = new Task();
                Task.Date = Convert.ToDateTime(matches[i].ToString());
                Task.User = matches[i + 1].ToString();
                Task.Description = matches[i + 2].ToString();
                Task.ProjectID = matches[i + 3].ToString();
                Task.Id = matches[i + 4].ToString();

                i += 4;

                TasksTotal.Add(Task);
            }
            return TasksTotal;
        }

        private List<Task> SortTasksAscending(ObservableCollection<Task> tasksTotal)
        {
            List<Task> result = tasksTotal.OrderBy(x => (x.Date)).ToList<Task>();
            return result;
        }

        /// <summary>
        /// Counts total amount of Days worked
        /// </summary>
        /// <param name="tasksTotal"></param>
        /// <returns></returns>
        public int GetTotalWorkDayCount(ObservableCollection<Task> tasksTotal)
        {
            List<Task> sortedTasks = tasksTotal.ToList();
            sortedTasks.Sort();
            int i = tasksTotal.Count();
            int count = 0;
            DateTime dateTime = sortedTasks.First().Date;
            while(i>=0 ) 
            {
                int dayOccuranceCount = 0;
                foreach(Task task in sortedTasks)
                {
                    if (dateTime.Equals(task.Date))
                        dayOccuranceCount++;
                }
                if (dayOccuranceCount >= 1)
                    count++;
                i--;
                dateTime = dateTime.AddDays(1);
            }
            return count;
        } 
        public int GetTotalWeekCount (ObservableCollection<Task> tasksTotal)
        {
            List<Task> sortedTasks = tasksTotal.ToList();
            sortedTasks.Sort();

            int startWeek = GetFirstOrLastCalenderWeek(sortedTasks, true);

            int endWeek = GetFirstOrLastCalenderWeek(sortedTasks, false);

            return endWeek - startWeek + 1;
        }

        private int GetFirstOrLastCalenderWeek(List<Task> tasksTotal, bool isFirst)
        {
            DateTime time = tasksTotal.First().Date;
            if (!isFirst)
            {
                time = tasksTotal.Last().Date;
            }

            return GetCalenderWeek(time);

        }

        private int GetCalenderWeek( DateTime time)
        {
            if (time.DayOfWeek >= DayOfWeek.Monday && time.DayOfWeek <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public List<WorkWeek> MakeWorkWeeks(ObservableCollection<Task> tasksTotal)
        {
            List<Task> sortedTasks = tasksTotal.ToList();
            sortedTasks.Sort();
            List<WorkWeek> weeksTotal = new List<WorkWeek>();
            List<Task> tasksPerWeek = new List<Task>();
            int i = 0;
            int compareWeek = GetCalenderWeek(sortedTasks.First().Date);
            
            while(i < tasksTotal.Count())
            {
                while(i < tasksTotal.Count() && compareWeek.Equals(GetCalenderWeek(sortedTasks[i].Date)))
                {
                    tasksPerWeek.Add(sortedTasks[i]);
                    if(i< tasksTotal.Count())
                        i++;
                }
                weeksTotal.Add(MakeWorkWeek(tasksPerWeek));
                tasksPerWeek.Clear();
                if (i < tasksTotal.Count())
                    compareWeek = GetCalenderWeek(sortedTasks[i].Date);
            }

            return weeksTotal;
        }

        private WorkWeek MakeWorkWeek(List<Task> sortedTasks)
        {
            WorkWeek week = new WorkWeek();
            week.WorkDay[(int)Weekdays.Monday] = new ObservableCollection<Task>();
            week.WorkDay[(int)Weekdays.Tuesday] = new ObservableCollection<Task>();
            week.WorkDay[(int)Weekdays.Wednesday] = new ObservableCollection<Task>();
            week.WorkDay[(int)Weekdays.Thursday] = new ObservableCollection<Task>();
            week.WorkDay[(int)Weekdays.Friday] = new ObservableCollection<Task>();

            foreach (Task task in sortedTasks)
            {

                switch (task.Date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        week.WorkDay[(int)Weekdays.Monday].Add(task);
                        break;
                    case DayOfWeek.Tuesday:
                        week.WorkDay[(int)Weekdays.Tuesday].Add(task);
                        break;
                    case DayOfWeek.Wednesday:
                        week.WorkDay[(int)Weekdays.Wednesday].Add(task);
                        break;
                    case DayOfWeek.Thursday:
                        week.WorkDay[(int)Weekdays.Thursday].Add(task);
                        break;
                    case DayOfWeek.Friday:
                        week.WorkDay[(int)Weekdays.Friday].Add(task);
                        break;
                }
            }
            return week;
        }
    }
}

