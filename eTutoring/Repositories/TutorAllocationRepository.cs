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

        public async Task<IEnumerable<UserResponseModelDto>> GetUnallocatedStudents()
        {
            var allocatedStudentIds = _authContext.TutorAllocations.Select(
                allocation => allocation.StudentId
            );
            var students = await _authRepository.AllStudents();
            var unallocatedStudents = from student in students
                                      where !allocatedStudentIds.Contains(student.Id)
                                      select student.ToUserResponseModel();
            return unallocatedStudents;
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
            return ToAllocationResponses();
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

        public IEnumerable<AllocationResponseModel> ToAllocationResponses()
        {
            var allocations = from allocation in _authContext.TutorAllocations
                              join tutor in _authContext.Users on allocation.TutorId equals tutor.Id
                              join student in _authContext.Users on allocation.StudentId equals student.Id
                              select new { tutor, student };
            var allocationGroup = from allocation in allocations
                                  group allocation by allocation.tutor into one_group
                                  select one_group;

            var result = new List<AllocationResponseModel>();
            foreach (var oneGroup in allocationGroup)
            {
                var students = oneGroup.Select(data => data.student.ToUserResponseModel());

                result.Add(new AllocationResponseModel
                {
                    Tutor = oneGroup.Key.ToUserResponseModel(),
                    Students = students
                });
            }

            // merge tutors with no allocations
            var allocatedTutorIds = from allocation in _authContext.TutorAllocations
                                    select allocation.TutorId;
            var unallocatedTutors = from tutor in _authRepository.AllTutors()
                                    where !allocatedTutorIds.Contains(tutor.Id)
                                    select tutor.ToUserResponseModel();
            foreach (var tutor in unallocatedTutors)
            {
                result.Add(new AllocationResponseModel
                {
                    Tutor = tutor,
                    Students = Enumerable.Empty<UserResponseModelDto>()
                });
            }


            return result;
        }
    }
}