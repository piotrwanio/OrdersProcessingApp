using CoreServicesBootcamp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.DAL;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class WholeRequestService
    {
        private RequestContext _context;

        public WholeRequestService(RequestContext context)
        {
            _context = context;
        }

        public List<WholeRequest> GetWholeRequestsByClient(int clientId)
        {
            List<WholeRequest> wholeRequests = GetAllWholeRequests();

            return (from r in wholeRequests
                    where r.ClientId == clientId
                    select r).ToList();
        }

        public List<WholeRequest> GetAllWholeRequests()
        {
            List<WholeRequest> wholeRequests = new List<WholeRequest>();

            var allFromRepo = _context.Requests;
            //var duplicates = (from r in allFromRepo
            //                  group r by new { r.ClientId, r.RequestId } into results
            //                  select results.Skip(1)
            //     ).SelectMany(a => a).ToList();


            //foreach (var dup in duplicates)
            //{
            //    allFromRepo.Remove(dup);
            //}

            foreach(var req in allFromRepo)
            {
                var contains = (from r in wholeRequests
                                where r.ClientId == req.ClientId
                                && r.RequestId == req.RequestId
                                select r);
                if (contains.Count() != 0)
                {
                    contains.First().RequestsList.Add(req);
                }
                else
                {
                    WholeRequest wholeRequest = new WholeRequest();
                    wholeRequest.ClientId = req.ClientId;
                    wholeRequest.RequestId = req.RequestId;
                    wholeRequest.RequestsList = new List<Request>();
                    wholeRequest.RequestsList.Add(req);
                    wholeRequests.Add(wholeRequest);
                }
            }
            return wholeRequests;
        }
    }
}
