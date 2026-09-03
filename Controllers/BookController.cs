using LiveProject_WebAPI_Demo.Data;
using LiveProject_WebAPI_Demo.DTOs;
using LiveProject_WebAPI_Demo.Models;
using LiveProject_WebAPI_Demo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveProject_WebAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public BookController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;

        }
        
        /* SYNC PROGRAMMING */
        /*[HttpGet]
        public IActionResult GetBooks()
        {
            var books = _context.Books.ToList();

            return Ok(books);
        }*/
        
        
        
        /* ASYNC PROGRAMMING */
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            return Ok(books);
        }
        
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updateBook)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null)
            {
                return NotFound();
            }

            book.Name = updateBook.Name;
            book.Author = updateBook.Author;
            book.Price = updateBook.Price;
            book.ImageUrl = updateBook.ImageUrl;
            
            

            await _context.SaveChangesAsync();

            return Ok(book);
        }
        
        
        
        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
           await _context.Books.AddAsync(book);

           await _context.SaveChangesAsync();


            return StatusCode(StatusCodes.Status201Created, book);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);

           await _context.SaveChangesAsync();

            return Ok("Book Deleted Successfully!!!");

        }



        [HttpPost("upload-book-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var imageUrl = await _cloudinaryService.UploadImageAsync(image);

            return Ok(new
            {
                Message = "Image Uploaded Successfully!!",
                SecureImage_Url = imageUrl
            });
        }



        [HttpPost("with-image")]
        public async Task<IActionResult> AddBookWithImage([FromForm] AddBookWithImageRequest request)
        {
            var imageUrl = await _cloudinaryService.UploadImageAsync(request.Image!);


            var book = new Book
            {
                Name = request.Name,
                Author = request.Author,
                Price = request.Price,
                ImageUrl = imageUrl
            };

            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, book);

        }
        
        
        /*[HttpPost("with-image")]
        public async Task<IActionResult> AddBookWithImage([FromForm] AddBookWithImageRequest request)
        {
            try
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(request.Image!);


                var book = new Book
                {
                    Name = request.Name,
                    Author = request.Author,
                    Price = request.Price,
                    ImageUrl = imageUrl
                };

                await _context.Books.AddAsync(book);

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, book);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "Something went wrong",/*
                       error = ex.Message#1#
                    });
            }

        }*/
        
        
    }
}





    


