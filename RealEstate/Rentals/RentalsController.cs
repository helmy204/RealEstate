using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RealEstate.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;

namespace RealEstate.Rentals
{
    public class RentalsController : Controller
    {
        public readonly RealEstateContext Context = new RealEstateContext();

        //public ActionResult Index()
        //{
        //    var rentals = Context.Rentals.FindAll();
        //    return View(rentals);
        //}

        //public ActionResult Index(RentalsFilter filters)
        //{
        //    var rentals = FilterRentals(filters)
        //        .SetSortOrder(SortBy<Rental>.Ascending(r => r.Price));
        //    var model = new RentalsList
        //    {
        //        Rentals = rentals,
        //        Filters = filters
        //    };
        //    return View(model);
        //}

        public ActionResult Index(RentalsFilter filters)
        {
            var rentals = FilterRentals(filters);
            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filters
            };
            return View(model);
        }

        private IEnumerable<Rental> FilterRentals(RentalsFilter filters)
        {
            IQueryable<Rental> rentals = Context.Rentals.AsQueryable()
                .OrderBy(r => r.Price);

            if (filters.MinimumRooms.HasValue)
            {
                rentals = rentals.Where(r => r.NumberOfRooms >= filters.MinimumRooms);
            }

            if (filters.PriceLimit.HasValue)
            {
                var query = Query<Rental>.LTE(r => r.Price, filters.PriceLimit);
                rentals = rentals.Where(r => query.Inject());
            }

            return rentals;
        }


        //private MongoCursor<Rental> FilterRentals(RentalsFilter filters)
        //{
        //    if (!filters.PriceLimit.HasValue)
        //    {
        //        return Context.Rentals.FindAll();
        //    }

        //    var query = Query<Rental>.LTE(r => r.Price, filters.PriceLimit);
        //    return Context.Rentals.Find(query);
        //}

        //
        // GET: /Rentals/Post
        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            Context.Rentals.Insert(rental);
            return RedirectToAction("Index");
        }

        public ActionResult AdjustPrice(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }

        private Rental GetRental(string id)
        {
            var rental = Context.Rentals.FindOneById(new ObjectId(id));
            return rental;
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            Context.Rentals.Save(rental);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Context.Rentals.Remove(Query.EQ("_id", new ObjectId(id)));
            return RedirectToAction("Index");
        }

        public string PriceDistribution()
        {
            return new QueryPriceDistribution()
              .Run(Context.Rentals)
              .ToJson();
        }

        public ActionResult AttachImage(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = GetRental(id);
            var imageId = ObjectId.GenerateNewId();
            rental.ImageId = imageId.ToString();
            Context.Rentals.Save(rental);
            var options = new MongoGridFSCreateOptions
            {
                Id = imageId,
                ContentType = file.ContentType
            };
            Context.Database.GridFS.Upload(file.InputStream, file.FileName);
            return RedirectToAction("Index");
        }
    }
}