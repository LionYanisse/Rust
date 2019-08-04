using Oxide.Core;
using Rust;
using System;
using UnityEngine;
namespace Oxide.Plugins
{
    [Info("KnockOut", "Owned", "1.0.1")]
    [Description("Knock out others players with your rock !")]

    class KnockOut : RustPlugin
    {
      const string permissionName = "knockout.use";
      private static System.Random random = new System.Random();
      private void Init()
      {
          permission.RegisterPermission(permissionName, this);
      }
      void OnEntityTakeDamage(BaseEntity entity, HitInfo info)
      {
        if (entity == null || info == null)
        {
          return;
        }
        if (entity is BasePlayer)
        {
          if (info.Initiator != null)
          {
            BasePlayer playerInitiator = info.Initiator as BasePlayer;
            if (!permission.UserHasPermission(playerInitiator.UserIDString, permissionName)) return;
            var hitArea = info?.boneArea ?? (HitArea) (-1);
            if((int)hitArea != -1)
            {
              if(hitArea.ToString() == "Head")
              {
                if(info.WeaponPrefab != null)
                {
                  var weapon = info.WeaponPrefab.ShortPrefabName;
                  if(weapon.ToString() == "rock.entity")
                  {
                    if (random.Next(0, 2) == 0)
                    {
                      BasePlayer player = entity as BasePlayer;
                      player.StartSleeping();
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
