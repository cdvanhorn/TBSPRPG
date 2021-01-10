using TbspRpgLib.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TbspRpgLib.Repositories {
    public interface IServiceRepository {
        List<Service> GetAllServices();

        Service GetServiceByName(string name);
    }

    public class ServiceRepository : IServiceRepository {
        List<Service> _services;

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
        }

        public List<Service> GetAllServices() {
            return _services;
        }

        public Service GetServiceByName(string name) {
            return _services.Where(ser => ser.Name == name.ToLower()).FirstOrDefault();
        }
    }
}
