using eTutoring.DbContext;
using eTutoring.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Repositories
{
    public class DashboardRepository : IDisposable
    {
        private readonly AuthContext _context = new AuthContext();

        public TutorDashboardStatResponseModel GetTutorStats(string tutorId)
        {
            var studentIds = from allocation in _context.TutorAllocations
                              where allocation.TutorId.Equals(tutorId)
                              select allocation.StudentId;
            var numberOfStudents = studentIds.Count();

            var submissions = from fileUploads in _context.FileUploads
                              where studentIds.Contains(fileUploads.AuthorId)
                              select fileUploads;
            var submissionCount = submissions.Count();
            return new TutorDashboardStatResponseModel
            {
                NumberOfFiles = submissionCount,
                NumberOfStudents = numberOfStudents
            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}