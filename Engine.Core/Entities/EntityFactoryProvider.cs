﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Engine.Entities
{
	public class EntityFactoryProvider
	{
		private readonly Dictionary<string, IEntityFactory> _entityFactories;

		public EntityFactoryProvider([InjectOptional] List<IEntityFactory> entityFactories)
		// TODO: remove zenject dependency when implicit optional collection paramters is implemented
		{
			_entityFactories = entityFactories.ToDictionary(k => k.Archetype.Name, v => v);
		}

		public bool TryCreateEntityFromArchetype(string archetypeName, out Entity entity)
		{
			IEntityFactory entityFactory;
			if (_entityFactories.TryGetValue(archetypeName, out entityFactory))
			{
				entity = entityFactory.CreateEntityFromArchetype();
				return true;
			}
			entity = null;
			return false;
		}
	}
}