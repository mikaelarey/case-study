using AssetManagement.BusinessLayer;
using AssetManagement.DataLayer;
using AssetManagement.Dto;
using AssetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AssetsDbContext _assetsDbContext;

        public AssetsController(AssetsDbContext assetsDbContext)
        {
            _assetsDbContext = assetsDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AssetDto>> GetAssets()
        {
            return BL_Assets.GetAssets();

            //return _assetsDbContext.Assets.Where(x => x.IsDeleted == 0).ToList();
        }

        [HttpGet("{AssetId:int}")]
        public async Task<ActionResult<Asset>> GetAssetById(int AssetId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Asset not found.");

            var asset = await _assetsDbContext.Assets.FindAsync(AssetId);

            if (asset == null)
                return NotFound();

            return asset;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsset(AssetDto asset)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (await _assetsDbContext.Assets.FirstOrDefaultAsync(x => x.Name == asset.Name && x.IsDeleted == 0) != null)
                return Conflict();

            var newAsset = new Asset
            {
                CreatedDate = DateTime.UtcNow.AddHours(8),
                DeletedDate = DateTime.MaxValue,
                IsDeleted = 0,
                Name = asset.Name,
                ValidFrom = asset.ValidFrom == null ? DateTime.MinValue : Convert.ToDateTime(asset.ValidFrom),
                ValidTo = asset.ValidTo == null ? DateTime.MaxValue : Convert.ToDateTime(asset.ValidTo)
            };

            await _assetsDbContext.Assets.AddAsync(newAsset);
            var isSuccessful = await _assetsDbContext.SaveChangesAsync() > 0;

            if (!isSuccessful)
                return StatusCode(500);

            var newAssetPrice = new AssetPrice
            {
                AssetId = newAsset.AssetId,
                EndDate = DateTime.MaxValue,
                Price = asset.Price,
                StartDate = DateTime.UtcNow.AddHours(8),
            };

            await _assetsDbContext.AssetPrice.AddAsync(newAssetPrice);
            await _assetsDbContext.SaveChangesAsync();

            return Created("/", newAsset);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsset(AssetDto asset)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var previousData = await _assetsDbContext.Assets.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == asset.AssetId);

            if (previousData == null)
                return NotFound();

            if (previousData.Name != asset.Name)
            {
                if (await _assetsDbContext.Assets.FirstOrDefaultAsync(x => x.Name == asset.Name) != null)
                    return Conflict();
            }

            var assetToUpdate = previousData;

            assetToUpdate.AssetId = asset.AssetId;
            assetToUpdate.Name = asset.Name;
            assetToUpdate.ValidFrom = asset.ValidFrom == null ? DateTime.MinValue : Convert.ToDateTime(asset.ValidFrom);
            assetToUpdate.ValidTo = asset.ValidTo == null ? DateTime.MaxValue : Convert.ToDateTime(asset.ValidTo);

            _assetsDbContext.Assets.Update(assetToUpdate);
            var isSuccessful = await _assetsDbContext.SaveChangesAsync() > 0;

            if (!isSuccessful)
                return StatusCode(500);

            var previousAssetPrice = await _assetsDbContext.AssetPrice
                                                           .OrderByDescending(x => x.AssetId)
                                                           .AsNoTracking()
                                                           .FirstOrDefaultAsync(x => x.AssetId == asset.AssetId);

            if (previousAssetPrice.Price != asset.Price)
            {
                var assetPriceToUpdate = previousAssetPrice;
                assetPriceToUpdate.EndDate = DateTime.UtcNow.AddHours(8);

                _assetsDbContext.AssetPrice.Update(assetPriceToUpdate);
                await _assetsDbContext.SaveChangesAsync();

                var newAssetPrice = new AssetPrice
                {
                    AssetId = asset.AssetId,
                    EndDate = DateTime.MaxValue,
                    Price = asset.Price,
                    StartDate = DateTime.UtcNow.AddHours(8),
                };

                await _assetsDbContext.AssetPrice.AddAsync(newAssetPrice);
                await _assetsDbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpDelete("{AssetId:int}")]
        public async Task<ActionResult> Delete(int AssetId)
        {
            var asset = await _assetsDbContext.Assets.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == AssetId);

            if (asset == null)
                return NotFound();

            //Update to IsDeleted instead of deleting the existing record.This is for future reference of invoice.
            _assetsDbContext.Assets.Remove(asset);
            _assetsDbContext.SaveChangesAsync().Wait();

            //var assetToDelete = asset;

            //assetToDelete.IsDeleted = 1;
            //assetToDelete.DeletedDate = DateTime.UtcNow.AddHours(8).Date;

            //_assetsDbContext.Update(assetToDelete);

            //var isSuccessful = await _assetsDbContext.SaveChangesAsync() > 0;

            //if (!isSuccessful)
            //    return BadRequest("Unable to delete asset");

            return Ok();
        }
    }
}
