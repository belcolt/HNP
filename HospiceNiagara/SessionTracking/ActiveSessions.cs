using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospiceNiagara.SessionTracking
{
    public class TrackInfo
    {
        private string _user;
        private  string _currentPage;

        public string User
        {
            get;
            set;
        }
        public string CurrentPage
        {
            get;
            set;
        }
    }

    public class ActiveSessions
    {

        
        private static Dictionary<string, TrackInfo> _sessionInfo;
        //private static List<string> _sessionEmail;
        private static readonly object padlock = new object();

        public static Dictionary<string,TrackInfo> Sessions
        {
            get
            {
                lock (padlock)
                {
                    if (_sessionInfo == null)
                    {
                        _sessionInfo = new Dictionary<string, TrackInfo>();
                    }
                    return _sessionInfo;
                }
            }
        }

        
        public static int Count
        {
            get
            {
                lock (padlock)
                {
                    if (_sessionInfo == null)
                    {
                        _sessionInfo = new Dictionary<string,TrackInfo>();
                    }
                    return _sessionInfo.Count;
                }
            }
        }
    }
}