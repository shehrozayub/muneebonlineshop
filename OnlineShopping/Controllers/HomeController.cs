using OnlineShopping.DAL;
using OnlineShopping.Filters;
using OnlineShopping.Repository;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    [FrontPageActionFilter]
    public class HomeController : Controller
    {
        #region Other Class references ... // Instance on Unit of Work 
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork(); 
        #endregion  
        /// <summary>
        /// Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.FeaturedProducts = _unitOfWork.GetRepositoryInstance<Tbl_Product>().GetListByParameter(i => i.IsFeatured == true && i.IsDelete==false && i.IsActive==true).ToList();
            return View();
        }
        
        #region Disposing UnitOfWork Context ...
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}