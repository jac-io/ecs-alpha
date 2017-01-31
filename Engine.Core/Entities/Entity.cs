﻿using System;
using Engine.Components;
//using Engine.Messaging;
using Engine.Serialization;
using Engine.Util;
using Zenject;

namespace Engine.Entities
{
	public delegate void EntityDelegate(Entity entity);

	public class Entity : ComponentContainer, IEquatable<Entity>
	{
		public int Id { get; protected set; }

		protected bool Disposed;

		public event EntityDelegate EntityDestroyed;

		public void Initialize(int id)
		{
			Id = id;
			Disposed = false;
			EntityDestroyed = null;
			for (var i = 0; i < Components.Length; i++)
			{
				Components[i] = null;
			}
		}

		#region Event registry

		private void RaiseEntityDestroyed()
		{
			// TODO: reconsider using the event for the entity container
			EntityDestroyed?.Invoke(this);
			//OnNext(new EntityDestroyedMessage(MessageScope.External, this));
			EntityDestroyed = null;
		}

		public override void Dispose()
		{
			//TODO: interlock this perhaps, once we want to support multithreading
			if (Disposed == false)
			{
				Disposed = true;
				RaiseEntityDestroyed();
				base.Dispose();
			}
		}

		~Entity()
		{
			Dispose();
		}

		#endregion

		public bool Equals(Entity other)
		{
			return Id == other?.Id;
		}

		public override string ToString()
		{
			return $"Entity [{Id}] {this.GetType()}";
		}

		public virtual void OnDeserialized()
		{
		}

		public class Factory : Factory<Entity>
		{
			
		}
	}
}
