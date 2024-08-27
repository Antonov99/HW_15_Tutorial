using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("enabled", "type", "mode", "dampen", "dampenMultiplier", "bounce", "bounceMultiplier", "lifetimeLoss", "lifetimeLossMultiplier", "minKillSpeed", "maxKillSpeed", "collidesWith", "enableDynamicColliders", "maxCollisionShapes", "quality", "voxelSize", "radiusScale", "sendCollisionMessages")]
	public class ES3Type_CollisionModule : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3Type_CollisionModule() : base(typeof(ParticleSystem.CollisionModule))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (ParticleSystem.CollisionModule)obj;
			
			writer.WriteProperty("enabled", instance.enabled);
			writer.WriteProperty("type", instance.type);
			writer.WriteProperty("mode", instance.mode);
			writer.WriteProperty("dampen", instance.dampen);
			writer.WriteProperty("dampenMultiplier", instance.dampenMultiplier);
			writer.WriteProperty("bounce", instance.bounce);
			writer.WriteProperty("bounceMultiplier", instance.bounceMultiplier);
			writer.WriteProperty("lifetimeLoss", instance.lifetimeLoss);
			writer.WriteProperty("lifetimeLossMultiplier", instance.lifetimeLossMultiplier);
			writer.WriteProperty("minKillSpeed", instance.minKillSpeed);
			writer.WriteProperty("maxKillSpeed", instance.maxKillSpeed);
			writer.WriteProperty("collidesWith", instance.collidesWith);
			writer.WriteProperty("enableDynamicColliders", instance.enableDynamicColliders);
			writer.WriteProperty("maxCollisionShapes", instance.maxCollisionShapes);
			writer.WriteProperty("quality", instance.quality);
			writer.WriteProperty("voxelSize", instance.voxelSize);
			writer.WriteProperty("radiusScale", instance.radiusScale);
			writer.WriteProperty("sendCollisionMessages", instance.sendCollisionMessages);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new ParticleSystem.CollisionModule();
			ReadInto<T>(reader, instance);
			return instance;
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			var instance = (ParticleSystem.CollisionModule)obj;
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					case "enabled":
						instance.enabled = reader.Read<Boolean>();
						break;
					case "type":
						instance.type = reader.Read<ParticleSystemCollisionType>();
						break;
					case "mode":
						instance.mode = reader.Read<ParticleSystemCollisionMode>();
						break;
					case "dampen":
						instance.dampen = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						break;
					case "dampenMultiplier":
						instance.dampenMultiplier = reader.Read<Single>();
						break;
					case "bounce":
						instance.bounce = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						break;
					case "bounceMultiplier":
						instance.bounceMultiplier = reader.Read<Single>();
						break;
					case "lifetimeLoss":
						instance.lifetimeLoss = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						break;
					case "lifetimeLossMultiplier":
						instance.lifetimeLossMultiplier = reader.Read<Single>();
						break;
					case "minKillSpeed":
						instance.minKillSpeed = reader.Read<Single>();
						break;
					case "maxKillSpeed":
						instance.maxKillSpeed = reader.Read<Single>();
						break;
					case "collidesWith":
						instance.collidesWith = reader.Read<LayerMask>();
						break;
					case "enableDynamicColliders":
						instance.enableDynamicColliders = reader.Read<Boolean>();
						break;
					case "maxCollisionShapes":
						instance.maxCollisionShapes = reader.Read<Int32>();
						break;
					case "quality":
						instance.quality = reader.Read<ParticleSystemCollisionQuality>();
						break;
					case "voxelSize":
						instance.voxelSize = reader.Read<Single>();
						break;
					case "radiusScale":
						instance.radiusScale = reader.Read<Single>();
						break;
					case "sendCollisionMessages":
						instance.sendCollisionMessages = reader.Read<Boolean>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}
}