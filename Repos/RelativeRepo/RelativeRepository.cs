using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using System;
using System.Linq.Expressions;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;

namespace ZahimarProject.Repos.RelativeRepo
{
    public class RelativeRepository :Repository<Relative> , IRelativeRepository
    {
        public readonly Context context;
        
        internal  DbSet<Relative> Relatives;
        public RelativeRepository(Context _context):base (_context)
        {
            this.context = _context;
            this.Relatives = context.Relatives;
          
        }
        public string GetMailOfRelativeToSendEmail(int PatientId)
        {
            Relative relative = Relatives.Include(r=>r.AppUser).FirstOrDefault(r=>r.PatientId==PatientId);
            return relative.AppUser.Email;
        }
        public Relative GetRelative(int PatientId)
        {
            Relative relative = Relatives.Include(r=>r.AppUser).FirstOrDefault(r => r.PatientId == PatientId);
            return relative;
        }

        public override Relative Get(Expression<Func<Relative, bool>> filter)
        {
            return Relatives.Include(r=>r.Patient).FirstOrDefault(filter);
        }

        public override List<Relative> GetAll()
        {
            return Relatives.Include(r => r.Patient).ToList();
        }
       

        
    }
}
