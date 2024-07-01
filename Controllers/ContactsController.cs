using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OWASPTop10TaskManager.Data;
using OWASPTop10TaskManager.Models;
using OWASPTop10TaskManager.Utils;

namespace OWASPTop10TaskManager.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService AuthorizationService;
        private readonly UserManager<IdentityUser> UserManager;

        public ContactsController(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }


        // Secure Code: This code improves security by filtering contacts based on user roles
        // and contact status to prevent unauthorized access to sensitive data.
        public async Task<IActionResult> IndexSecure()
        {
            var contacts = from c in _context.Contacts
                           select c;

            var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
                               User.IsInRole(Constants.ContactAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                contacts = contacts.Where(c => c.Status == ContactStatus.Approved
                                            || c.OwnerID == currentUserId);
            }
            return View(await contacts.ToListAsync());
        }


        // Vulnerable Code: This code retrieves all contacts from the database
        // without any filtering based on user authorization or contact status.
        // This exposes sensitive data to unauthorized users.
        public async Task<IActionResult> Index()
        {
            var contacts = from c in _context.Contacts
                           select c;

            
            return View(await contacts.ToListAsync());
        }

        //[AllowAnonymous]
        //// GET: Contacts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts
        //        .FirstOrDefaultAsync(m => m.ContactId == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!User.Identity!.IsAuthenticated)
        //    {
        //        return Challenge();
        //    }

        //    var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
        //                       User.IsInRole(Constants.ContactAdministratorsRole);

        //    var currentUserId = UserManager.GetUserId(User);

        //    if (!isAuthorized
        //        && currentUserId != contact.OwnerID
        //        && contact.Status != ContactStatus.Approved)
        //    {
        //        return Forbid();
        //    }

        //    return View(contact);
        //}

        //// GET: Contacts/Details/5
        //public async Task<IActionResult> DetailsSecure(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts
        //        .FirstOrDefaultAsync(m => m.ContactId == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }


        //    var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
        //                   User.IsInRole(Constants.ContactAdministratorsRole);

        //    var currentUserId = UserManager.GetUserId(User);

        //    if (!isAuthorized
        //        && currentUserId != contact.OwnerID
        //        && contact.Status != ContactStatus.Approved)
        //    {
        //        return Forbid();
        //    }
        //    return View(contact);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Details(int id, ContactStatus status)
        //{
        //    var contact = await _context.Contacts.FirstOrDefaultAsync(
        //                                              m => m.ContactId == id);

        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactOperation = (status == ContactStatus.Approved)
        //                                               ? ContactOperations.Approve
        //                                               : ContactOperations.Reject;

        //    var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact,
        //                                contactOperation);
        //    if (!isAuthorized.Succeeded)
        //    {
        //        return Forbid();
        //    }
        //    contact.Status = status;
        //    _context.Contacts.Update(contact);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("./Index");
        //}

        //// GET: Contacts/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Contacts/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ContactId,Name,Address,City,State,Zip,Email")] Contact contact)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        contact.OwnerID = UserManager.GetUserId(User);

        //        var isAuthorized = await AuthorizationService.AuthorizeAsync(
        //                                                    User, contact,
        //                                                    ContactOperations.Create);
        //        if (!isAuthorized.Succeeded)
        //        {
        //            return Forbid();
        //        }

        //        _context.Add(contact);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contact);
        //}

        //// GET: Contacts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts.FindAsync(id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    var isAuthorized = await AuthorizationService.AuthorizeAsync(
        //                                              User, contact,
        //                                              ContactOperations.Update);
        //    if (!isAuthorized.Succeeded)
        //    {
        //        return Forbid();
        //    }

        //    return View(contact);
        //}

        //// POST: Contacts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ContactId,Name,Address,City,State,Zip,Email")] Contact contact)
        //{
        //    if (id != contact.ContactId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(contact);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContactExists(contact.ContactId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contact);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditSecure(int id, [Bind("ContactId,Name,Address,City,State,Zip,Email")] Contact contact)
        //{
        //    if (id != contact.ContactId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Fetch Contact from DB to get OwnerID.
        //            var contact1 = await _context
        //                .Contacts.AsNoTracking()
        //                .FirstOrDefaultAsync(m => m.ContactId == id);

        //            if (contact == null)
        //            {
        //                return NotFound();
        //            }

        //            var isAuthorized = await AuthorizationService.AuthorizeAsync(
        //                                         User, contact,
        //                                         ContactOperations.Update);
        //            if (!isAuthorized.Succeeded)
        //            {
        //                return Forbid();
        //            }

        //            contact.OwnerID = contact1!.OwnerID;

        //            _context.Attach(contact).State = EntityState.Modified;

        //            if (contact1.Status == ContactStatus.Approved)
        //            {
        //                // If the contact is updated after approval, 
        //                // and the user cannot approve,
        //                // set the status back to submitted so the update can be
        //                // checked and approved.
        //                var canApprove = await AuthorizationService.AuthorizeAsync(User,
        //                                        contact,
        //                                        ContactOperations.Approve);

        //                if (!canApprove.Succeeded)
        //                {
        //                    contact1.Status = ContactStatus.Submitted;
        //                }
        //            }

        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContactExists(contact.ContactId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contact);
        //}

        //// GET: Contacts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts
        //        .FirstOrDefaultAsync(m => m.ContactId == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contact);
        //}


        //public async Task<IActionResult> DeleteSecure(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts
        //        .FirstOrDefaultAsync(m => m.ContactId == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    var isAuthorized = await AuthorizationService.AuthorizeAsync(
        //                                         User, contact,
        //                                         ContactOperations.Delete);
        //    if (!isAuthorized.Succeeded)
        //    {
        //        return Forbid();
        //    }

        //    return View(contact);
        //}

        //// POST: Contacts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var contact = await _context.Contacts.FindAsync(id);
        //    if (contact != null)
        //    {
        //        _context.Contacts.Remove(contact);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost, ActionName("DeleteSecure")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmedSecure(int id)
        //{
        //    var contact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(m => m.ContactId == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    if (contact != null)
        //    {
        //        _context.Contacts.Remove(contact);
        //    }

        //    var isAuthorized = await AuthorizationService.AuthorizeAsync(
        //                                        User, contact,
        //                                        ContactOperations.Delete);
        //    if (!isAuthorized.Succeeded)
        //    {
        //        return Forbid();
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ContactExists(int id)
        //{
        //    return _context.Contacts.Any(e => e.ContactId == id);
        //}
    }
}
