using CoreServicesBootcamp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace CoreServicesBootcamp.DAL.Repositories
{
    public class RequestRepository
    {
        private RequestContext _context;

        public RequestRepository(RequestContext context)
        {
            _context = context;
        }

        public void AddRequest(Request request)
        {
            _context.Add(request);
            _context.SaveChanges();

        }

        public int AllRequestsCount()
        {
            Debug.WriteLine(GetAllRequests().Count);

            return GetAllRequests().Count();
        }

        public int ClientRequestCount(int id)
        {
            Debug.WriteLine(GetClientRequests(id).Count());

            return GetClientRequests(id).Count();
        }

        public List<Request> GetAllRequests()
        {
            //var duplicates = (from r in _context.Requests
            //                  group r by new { r.ClientId, r.RequestId } into results
            //                  select results.Skip(1)
            //                 ).SelectMany(a => a).ToList();

            var all = (from r in _context.Requests select r).ToList();

            //foreach (var dup in duplicates)
            //{
            //    all.Remove(dup);
            //}
 
            return all;
        }

        public List<Request> GetClientRequests(int id)
        {
            var clientRequests = (from r in _context.Requests
                                  where r.ClientId == id
                                  select r).ToList();

            var duplicates = (from r in _context.Requests
                              where r.ClientId == id
                              group r by new { r.ClientId, r.RequestId } into results
                              select results.Skip(1)).SelectMany(a => a).ToList();

            foreach (var dup in duplicates)
            {
                clientRequests.Remove(dup);
            }

            return clientRequests;
        }
    }
}
