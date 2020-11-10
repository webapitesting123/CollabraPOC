using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FabrikamResidences_Activities.Models;

namespace FabrikamResidences_Activities.Data
{
    public class PortalRepository_Memory : IPortalRepository
    {
        private List<PortalActivity> _activities;
        private List<Attendee> _attendees;

        public PortalRepository_Memory()
        { }

        #region Manage Database Schema
        
        public void InitializeDatabase()
        {
            if (!IsSchemaReady())
            {
#if (DEBUG)
                SeedData();
#else
                InitialzeLists();
#endif
            }
        }

        private bool IsSchemaReady()
        {
            return _activities != null || _attendees != null;
        }

        private void InitialzeLists()
        {
            _activities = new List<PortalActivity>();
            _attendees = new List<Attendee>();
        }

        private void SeedData()
        {
            _activities = SeedActivity();
            _attendees = SeedAttendee();
        }

        private List<PortalActivity> SeedActivity()
        {
            DateTime now = DateTime.Now;
            return new List<PortalActivity> {
                new PortalActivity()
                {
                    Id = 1,
                    Name = "Bingo",
                    Description = "Come join us for an exciting game of Bingo with great prizes.",
                    Date = new DateTime(now.Year, now.Month, now.AddDays(2).Day, 12, 00, 00)
                },
                new PortalActivity()
                {
                    Id = 2,
                    Name = "Shuffleboard Competition",
                    Description = "Meet us at the Shuffleboard court!",
                    Date = new DateTime(now.Year, now.Month, now.AddDays(5).Day, 18, 00, 00)
                }
            };
        }

        private List<Attendee> SeedAttendee()
        {
            int i = 1;
            return new List<Attendee> {
                new Attendee()
                {
                    Id = i++,
                    PortalActivityId = 1,
                    FirstName = "Joe",
                    LastName = "Bingo",
                    Email = "Joe@Addict.com"
                },
                new Attendee()
                {
                    Id = i++,
                    PortalActivityId = 1,
                    FirstName = "john",
                    LastName = "doe",
                    Email = "jdoe@anonymous.com"
                },
                new Attendee()
                {
                    Id = i++,
                    PortalActivityId = 2,
                    FirstName = "Jill",
                    LastName = "Hill",
                    Email = "champ@shuffleboard.com"
                },
                new Attendee()
                {
                    Id = i++,
                    PortalActivityId = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@anonymous.com"
                }
            };
        }
        
        #endregion
        
        #region IPortalRepository Members
        
        public PortalActivity GetActivity(int id)
        {
            var activity = _activities.Find(a => a.Id == id);
            activity.Attendees = GetActivityAttendees(activity.Id).ToList();

            return activity;
        }

        public IQueryable<PortalActivity> GetActivities()
        {
            return _activities.AsQueryable();
        }
        
        public void AddActivity(PortalActivity activity)
        {
            _activities.Add(activity);
        }

        public void DeleteActivity(int id)
        {
            var activity = GetActivity(id);
            _activities.Remove(activity);
        }

        public void UpdateActivity(PortalActivity updActivity)
        {
            var currActivity = GetActivity(updActivity.Id);
            currActivity.Name = updActivity.Name;
            currActivity.Description = updActivity.Description;
            currActivity.Date = updActivity.Date;
        }

        public void AddAttendee(Attendee attendee)
        {
            _attendees.Add(attendee);
        }

        public IQueryable<Attendee> GetActivityAttendees(int activityId)
        {
            return _attendees.FindAll(a => a.PortalActivityId == activityId).AsQueryable();
        }
    
        #endregion 
    }
}