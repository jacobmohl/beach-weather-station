// az login
// az account set --subscription "Visual Studio Professional Subscription"
// az deployment group create --resource-group rg-jamobeachweatherstation-test --template-file server/iac/main.bicep --parameters environment=test
// az deployment group create --resource-group rg-jamobeachweatherstation-prod --template-file server/iac/main.bicep --parameters environment=prod

param location string = resourceGroup().location
param environment string = 'test'

var projectName string = 'beachweatherstation'

// Shared ressources

resource cosmosServer 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' existing = {
  name: 'cosmos-jamoshared-prod'
  scope: resourceGroup('rg-jamoshared-prod')
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' existing = {
  name: 'log-jamoshared-prod'
  scope: resourceGroup('rg-jamoshared-prod')
}


// Managed Identity
resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: 'id-jamo${projectName}-${environment}'
  location: location
}

// Application Inssights
resource appInsightsComponents 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: 'ins-jamo${projectName}-${environment}'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
  }
}


resource azureFunctionPlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: 'plan-jamo${projectName}-${environment}'
  location: location
  kind: 'functionapp'
  sku: {
    name: 'Y1'
  }
  properties: {}
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: 'stbeachfunc${environment}'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

// resource signalR 'Microsoft.SignalRService/signalR@2022-02-01' = {
//   name: 'signal-jamo${projectName}-prod'
//   location: location
//   kind: 'SignalR'
//   sku: {
//     name: 'Free_F1'
//     tier: 'Free'
//   }
//   properties: {
//     cors: {
//       allowedOrigins: [
//         '*'
//       ]
//     }
//     features: [
//       {
//         flag: 'ServiceMode'
//         value: 'Serverless'
//       }
//     ]
//   }
// }

// resource certificates 'Microsoft.Web/certificates@2022-03-01' = {
//   name: 'iot.jacobmohl.dk-func-jamobeachweatherstationapi-prod'
//   location: location
//   properties: {
//     canonicalName: 'iot.jacobmohl.dk'
//     serverFarmId: azureFunctionPlan.id
//   }
// }

resource azureFunction 'Microsoft.Web/sites@2020-12-01' = {
  name: 'func-jamo${projectName}api-${environment}'
  location: location
  kind: 'functionapp'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identity.id}': {}
    }
  }
  properties: {
    serverFarmId: azureFunctionPlan.id
    httpsOnly: false
    
    
    siteConfig: {
      cors: {
        allowedOrigins: [
          'https://badevand.jacobmohl.dk'
        ]
      }
      ftpsState: 'Disabled'
      http20Enabled: true
      webSocketsEnabled: true
      netFrameworkVersion: 'v9.0'
      use32BitWorkerProcess: false
      appSettings: [
        {
          name: 'AzureWebJobsDashboard'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${storageaccount.listKeys().keys[0].value}'
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${storageaccount.listKeys().keys[0].value}'
        }
        {
          name:'AzureFunctionsWebHost__hostid'
          value: '${projectName}api${environment}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${storageaccount.listKeys().keys[0].value}'
        }
        // {
        //   name: 'WEBSITE_CONTENTSHARE'
        //   value: toLower('name')
        // }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsightsComponents.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${appInsightsComponents.properties.InstrumentationKey}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        // {
        //   name: 'AzureSignalRConnectionString'
        //   value: signalR.listKeys().primaryConnectionString
        // }
        {
          name: 'CosmosDb:AccountEndpoint'
          value: 'https://${cosmosServer.name}.documents.azure.com:443'
        }
        {
          name: 'CosmosDb:AccountKey'
          value: cosmosServer.listKeys().primaryMasterKey
        } 
        {
          name: 'CosmosDb:DatabaseName'
          value: 'BeachWeatherStation'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '0'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
        {
          name: 'WEBSITE_TIME_ZONE'
          value: 'Romance Standard Time'
        }
      ]
    }
  }

//   resource domain 'hostNameBindings' = {
//     name: 'iot.jacobmohl.dk'
//     properties: {
//       siteName: 'func-jamo${projectName}api-prod'
//       hostNameType: 'Verified'
//       sslState: 'SniEnabled'
//       thumbprint: certificates.properties.thumbprint
//     }  
//   }

}


// resource staticWebApp 'Microsoft.Web/staticSites@2021-03-01' = {
//   name: 'static-jamo${projectName}web-${environment}'
//   location: location
//   sku: {
//     name: 'Free'
//     tier: 'Free'
//   }
//   properties: {
//     provider: 'GitHub'
//     repositoryUrl: 'https://github.com/jacobmohl/BeachWeatherStation'
//     branch: 'main'
//   }

//   resource domain 'customDomains' = {
//     name: 'badevand.jacobmohl.dk'
//   }
  
// }




// var functionPrincipalId = '011c4fad-b8e4-4310-957c-4b8bda37ec50'

// @description('This is the built-in SignalR App Server Role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#contributor')
// resource signalRAppServiceRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
//   scope: subscription()
//   name: '420fcaa2-552c-430f-98ca-3264be4806c7'
// }

// resource roleAssignmentFunctionToSignalR 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
//   scope: signalRComponent
//   name: guid(azureFunction.id, functionPrincipalId, signalRAppServiceRoleDefinition.id)
//   properties: {
//     roleDefinitionId: signalRAppServiceRoleDefinition.id
//     principalId: functionPrincipalId
//     principalType: 'ServicePrincipal'
//   }
// }
