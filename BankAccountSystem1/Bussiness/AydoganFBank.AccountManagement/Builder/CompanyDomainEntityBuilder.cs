﻿using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Builder
{
    public class CompanyDomainEntityBuilder : IDomainEntityBuilder<CompanyDomainEntity, Company>
    {
        #region  IoC

        private readonly ICoreContext coreContext;

        public CompanyDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        public CompanyDomainEntity MapToDomainObject(Company entity)
        {
            if (entity == null)
                return null;

            CompanyDomainEntity domainEntity = coreContext.New<CompanyDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(CompanyDomainEntity domainEntity, Company entity)
        {
            if (domainEntity == null || entity == null)
                return;

            domainEntity.Address = entity.Address;
            domainEntity.CompanyId = entity.CompanyId;
            domainEntity.CompanyName = entity.CompanyName;
            domainEntity.PhoneNumber = entity.PhoneNumber;
            domainEntity.ResponsablePerson = coreContext.Query<IPersonRepository>().GetById(entity.ResponsablePersonId);
            domainEntity.TaxNumber = entity.TaxNumber;
        }

        public IEnumerable<CompanyDomainEntity> MapToDomainObjectList(IEnumerable<Company> entities)
        {
            List<CompanyDomainEntity> companies = new List<CompanyDomainEntity>();
            foreach (var entity in entities)
            {
                companies.Add(MapToDomainObject(entity));
            }
            return companies;
        }
    }

    internal class CompanyMapper : IDbEntityMapper<Company, CompanyDomainEntity>
    {
        public void MapToDbEntity(CompanyDomainEntity domainEntity, Company dbEntity)
        {
            dbEntity.Address = domainEntity.Address;
            dbEntity.CompanyName = domainEntity.CompanyName;
            dbEntity.PhoneNumber = domainEntity.PhoneNumber;
            dbEntity.ResponsablePersonId = domainEntity.ResponsablePerson.PersonId;
            dbEntity.TaxNumber = domainEntity.TaxNumber;
        }
    }
}
