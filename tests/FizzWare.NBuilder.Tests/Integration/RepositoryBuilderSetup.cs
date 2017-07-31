﻿using System.Collections.Generic;
using FizzWare.NBuilder.Tests.Integration.Models;
using FizzWare.NBuilder.Tests.Integration.Models.Repositories;
using FizzWare.NBuilder.Tests.Integration.Support;

namespace FizzWare.NBuilder.Tests.Integration
{
    public class RepositoryBuilderSetup
    {
        private bool _setup;

        public RepositoryBuilderSetup()
        {
            DoSetup();
        }

        public BuilderSettings DoSetup()
        {
            new ProductRepository().DeleteAll();
            new CategoryRepository().DeleteAll();

            var builderSettings = new BuilderSettings();

            if (_setup)
                return builderSettings;


            _setup = true;

            var productRepository = Dependency.Resolve<IProductRepository>();
            var taxTypeRepository = Dependency.Resolve<ITaxTypeRepository>();
            var categoryRepository = Dependency.Resolve<ICategoryRepository>();

            builderSettings.SetCreatePersistenceMethod<Product>(productRepository.Create);
            builderSettings.SetCreatePersistenceMethod<IList<Product>>(productRepository.CreateAll);

            builderSettings.SetCreatePersistenceMethod<TaxType>(taxTypeRepository.Create);
            builderSettings.SetCreatePersistenceMethod<IList<TaxType>>(taxTypeRepository.CreateAll);

            builderSettings.SetCreatePersistenceMethod<Category>(categoryRepository.Create);
            builderSettings.SetCreatePersistenceMethod<IList<Category>>(categoryRepository.CreateAll);

            builderSettings.SetUpdatePersistenceMethod<Category>(categoryRepository.Save);
            builderSettings.SetUpdatePersistenceMethod<IList<Category>>(categoryRepository.SaveAll);
            return builderSettings;
        }
    }
}