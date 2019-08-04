using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Gather More Together", "Owned", "1.0.0")]
    [Description("Get more ressources when you're farming close to a team member")]

    class GatherMoreTogether : RustPlugin
    {
      private confData config;
      protected override void LoadDefaultConfig()
      {
          Config.WriteObject(new confData(),true);
      }
      private void Init()
      {
          config = Config.ReadObject<confData>();
      }
      private new void SaveConfig()
      {
          Config.WriteObject(config,true);
      }
      public class confData
      {
          [JsonProperty("Distance between you and your mates (in feet)")]
          public float distanceMate = 32f;

          [JsonProperty("Bonus percentage")]
          public int bonusMate = 10;
      }
      void OnDispenserGather(ResourceDispenser dispenser, BaseEntity entity, Item item)
      {
          if (entity.ToPlayer() == null) return;
          var basePlayer = entity.ToPlayer();
          var currentTeam = basePlayer.currentTeam;
          if(currentTeam != 0)
          {
              RelationshipManager.PlayerTeam team = RelationshipManager.Instance.FindTeam(currentTeam);
              if (team != null)
              {
                var players = team.members;
                if ( team.members.Count > 1 )
                {
                  foreach (var teamMember in players)
                  {
                      if(teamMember.ToString() != basePlayer.UserIDString)
                      {
                        BasePlayer member = BasePlayer.FindByID(teamMember);
                        if(member != null)
                        {
                          if(Vector3.Distance(basePlayer.transform.position, member.transform.position) <= config.distanceMate)
                          {
                            item.amount = item.amount + (item.amount * config.bonusMate/100);
                            basePlayer.ChatMessage("You got 10% more because you're close to a member of your team");
                          }
                        }
                      }
                  }
                }
                else {
                  return;
                }
              }
              else
              {
                return;
              }
          }
          else {
            return;
          }
      }
    }
}
