using Rvig.Base.App;
using System.Collections.Generic;
using System;
using Rvig.Data.Reisdocumenten.Mappers;
using Rvig.Data.Reisdocumenten.Repositories;
using Rvig.Data.Reisdocumenten.Services;
using Microsoft.AspNetCore.Builder;
using Rvig.BrpApi.Reisdocumenten.Interfaces;
using Rvig.BrpApi.Reisdocumenten.Validation.RequestModelValidators;
using Rvig.BrpApi.Reisdocumenten.Services;

var servicesDictionary = new Dictionary<Type, Type>
{
	// Data
	{ typeof(IRvigReisdocumentenRepo), typeof(RvigReisdocumentenRepo) },
	{ typeof(IRvIGDataReisdocumentenMapper), typeof(RvIGDataReisdocumentenMapper) },
	{ typeof(IGetAndMapGbaReisdocumentenService), typeof(GetAndMapGbaReisdocumentenService) },

	// API
	{ typeof(IGbaReisdocumentenApiService), typeof(GbaReisdocumentenApiService) }
};

var validatorList = new List<Type>
{
	typeof(RaadpleegMetReisdocumentnummerValidator),
	typeof(ZoekMetBurgerservicenummerValidator)
};

// This is used to give configurable options to deactive the authorization layer. This was determined by the Haal Centraal crew and the RvIG to be required.
// The reason for this requirement has to do with Kubernetes and a multi pod setup where another pod is responsible for authorizations therefore making this a sessionless API.
static bool UseAuthorizationLayer(WebApplicationBuilder builder)
{
	_ = bool.TryParse(builder.Configuration["ProtocolleringAuthorization:UseAuthorizationChecks"], out bool useAuthorizationChecks);

	return useAuthorizationChecks;
}

RvigBaseApp.Init(servicesDictionary, validatorList, UseAuthorizationLayer, "BRP Reisdocumenten API");