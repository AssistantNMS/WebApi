using NMS.Assistant.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMS.Assistant.Domain.Mapper
{
    public static class AppJsonFileMapper
    {

        /// <summary>
        /// </summary>
        /// <param name="appId"></param>
        /// <exception cref="ArgumentOutOfRangeException">If AppId is unknown</exception>
        /// <returns></returns>
        public static string FromAppId(string appId)
        {
            if (appId.Contains(AppIdPrefix.Building)) return AppJsonFile.Buildings;
            if (appId.Contains(AppIdPrefix.Cooking)) return AppJsonFile.Cooking;
            if (appId.Contains(AppIdPrefix.Curiosity)) return AppJsonFile.Curiosity;
            if (appId.Contains(AppIdPrefix.Other)) return AppJsonFile.Others;
            if (appId.Contains(AppIdPrefix.Product)) return AppJsonFile.Products;
            if (appId.Contains(AppIdPrefix.RawMaterial)) return AppJsonFile.RawMaterials;
            if (appId.Contains(AppIdPrefix.Technology)) return AppJsonFile.Technology;
            if (appId.Contains(AppIdPrefix.Trade)) return AppJsonFile.TradeItems;
            if (appId.Contains(AppIdPrefix.Upgrade)) return AppJsonFile.UpgradeModules;
            if (appId.Contains(AppIdPrefix.ConTech)) return AppJsonFile.ConstructedTechnology;
            if (appId.Contains(AppIdPrefix.ProcProd)) return AppJsonFile.ProceduralProducts;
            if (appId.Contains(AppIdPrefix.TechMod)) return AppJsonFile.TechnologyModule;

            throw new ArgumentOutOfRangeException($"AppId {appId} is invalid");
        }
    }
}
