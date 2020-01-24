using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class PictureAdded
    {
        public Guid RoomIncidentId { get; internal set; }
        public string Uri { get; internal set; }
        public PictureAdded(Guid roomIncidentId, string uri)
        {
            Uri = uri;
            RoomIncidentId = roomIncidentId;
        }
    }
}
