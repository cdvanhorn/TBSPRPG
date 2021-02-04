using TbspRpgLib.Entities;

using System;
using System.Linq;
using System.Collections.Generic;

namespace TbspRpgLib.Repositories {
    public interface IServiceRepository {
        List<Service> GetAllServices();

        Service GetServiceByName(string name);
        
        EventType GetEventTypeByName(string name);
    }

    public class ServiceRepository : IServiceRepository {
        List<Service> _services;

        List<EventType> _eventTypes;

        public ServiceRepository() {
            _services = new List<Service>();
            //create service objects
            _services.Add(new Service () {
                Id = new Guid("828786b0-e046-48a6-bb94-64b79bb24eaf"),
                Name = "game",
                Url = "http://gameapi:8003/api"
            });
            _services.Add(new Service () {
                Id = new Guid("94600c72-0d4c-4c9f-b247-56b366589534"),
                Name = "map",
                Url = "http://mapapi:8004/api"
            });
            _services.Add(new Service () {
                Id = new Guid("352ed331-75e2-4ab4-9a70-91cee0d2214c"),
                Name = "adventure",
                Url = "http://adventureapi:8002/api"
            });
            _services.Add(new Service () {
                Id = new Guid("1622685e-edaf-476c-8013-cde4453149a8"),
                Name = "gamesystem",
                Url = "http://gamesystemapi:8005/api"
            });

            _eventTypes = new List<EventType>();
            _eventTypes.Add(new EventType() {
                Id = new Guid("06072ad8-bf89-417a-8f9c-f8518ce16e70"),
                TypeName = "game_new"
            });
            _eventTypes.Add(new EventType() {
                Id = new Guid("ec17d133-fddf-4d53-bd5d-9f0f568167c7"),
                TypeName = "location_enter"
            });
            _eventTypes.Add(new EventType() {
                Id = new Guid("ac1b7ab7-b181-484b-8361-0a4f7b4581f6"),
                TypeName = "location_enter_check"
            });
            _eventTypes.Add(new EventType() {
                Id = new Guid("6fd70d83-1468-4499-88a1-e19a63753172"),
                TypeName = "location_enter_pass"
            });
            _eventTypes.Add(new EventType() {
                Id = new Guid("b46fc97c-6f35-4006-8af7-e048e9b9048b"),
                TypeName = "location_enter_fail"
            });
        }

        public List<Service> GetAllServices() {
            return _services;
        }

        public Service GetServiceByName(string name) {
            return _services.Where(ser => ser.Name == name.ToLower()).FirstOrDefault();
        }

        public EventType GetEventTypeByName(string name) {
            return _eventTypes.Where(et => et.TypeName == name.ToLower()).FirstOrDefault();
        }
    }
}
