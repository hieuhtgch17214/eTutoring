using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using eTutoring.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Repositories
{
    public class TutorAllocationRepository : IDisposable
    {
        private readonly AuthContext _authContext = new AuthContext();
        private readonly AuthRepository _authRepository = new AuthRepository();

        public async Task AllocateTutorToStudents(string tutorId, string[] studentIds)
        {
            var allocationModels = await CreateAllocationModels(tutorId, studentIds);
            foreach (var model in allocationModels)
            {
                _authContext.TutorAllocations.Add(model);
            }
            await _authContext.SaveChangesAsync();
        }

        public Task<int> DeallocateStudents(string[] studentIds)
        {
            var allocationList = _authContext.TutorAllocations.Where(
                allocation => studentIds.Contains(allocation.StudentId)
            );

            foreach (var data in allocationList)
            {
                _authContext.TutorAllocations.Remove(data);
            }

            return _authContext.SaveChangesAsync();
        }

        public IEnumerable<AllocationResponseModel> RetrieveAllAllocations()
        {
            return _authContext.ToAllocationResponses();
        }

        private async Task<IEnumerable<TutorAllocationModel>> CreateAllocationModels(string tutorId, string[] studentIds)
        {
            var tutor = await _authRepository.FindTutorsByIds(new string[] { tutorId });
            var students = await _authRepository.FindStudentsByIds(studentIds);
            if (tutor == null || tutor.Count == 0)
            {
                return Enumerable.Empty<TutorAllocationModel>();
            }
            if (students == null || students.Count == 0)
            {
                return Enumerable.Empty<TutorAllocationModel>();
            }

            return students.Select(student => new TutorAllocationModel
            {
                TutorId = tutorId,
                StudentId = student.Id
            });
        }

        public void Dispose()
        {
            _authRepository.Dispose();
            _authContext.Dispose();
        }
    }
}