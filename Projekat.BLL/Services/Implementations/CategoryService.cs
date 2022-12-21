using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Model;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.BLL.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public ResponsePackage<bool> Create(string categoryName)
        {
            try
            {
                _unitOfWork.Category.Add(new Category() { Name = categoryName });
                _unitOfWork.Save();
                return new ResponsePackage<bool>(true, ResponseStatus.OK, "Category added");

            }catch(Exception ex)
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while adding a category: " + ex.Message);
            }
        }

        public ResponsePackage<bool> Delete(int categoryId)
        {
            try
            {
                Category category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == categoryId);
                category.IsDeleted = true;
                _unitOfWork.Save();
                return new ResponsePackage<bool>(true, ResponseStatus.OK, "Category deleted");

            }
            catch (Exception ex)
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while deleting category: " + ex.Message);
            }
        }

        public ResponsePackage<IEnumerable<CategoryDTO>> GetAll()
        {
            IEnumerable<Category> lista= _unitOfWork.Category.GetAll();
            List<CategoryDTO> retList=new List<CategoryDTO>();
            foreach(Category category in lista)
            {
                if(category.IsDeleted==false)
                    retList.Add(new CategoryDTO { Category = category.Name, CategoryId = category.Id });
            }
                
            return new ResponsePackage<IEnumerable<CategoryDTO>>(retList, ResponseStatus.OK, "All categories");
        }

        public ResponsePackage<CategoryDTO> GetCategory(int? categoryId)
        {
            Category category = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==categoryId);
            if (category != null)
                return new ResponsePackage<CategoryDTO>(new CategoryDTO
                {
                    Category = category.Name,
                    CategoryId = category.Id
                }, ResponseStatus.OK, "Categoriey with Id:" + categoryId);
            else
                return new ResponsePackage<CategoryDTO>(null, ResponseStatus.CategoryNotFound, "Category not found");
        }


        public ResponsePackage<bool> Update(int categoryId,string newName)
        {
            try
            {
                Category category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == categoryId);
                category.Name = newName;
                _unitOfWork.Save();
                return new ResponsePackage<bool>(true, ResponseStatus.OK, "Category updated");

            }
            catch (Exception ex)
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while updating category: " + ex.Message);
            }
        }
    }
}
