# Gbit_learning
吉比特实训项目

策划须知
跳跃点这种按E键没有使用交互动画的：tag设置成QuickInteractive

## 程序TODO：
- [x] 战斗：进入战斗的房间不准切换房间/交互，打完怪物才能切换房间。
- [x] 背包改成5格，拼图不用显示在背包里
- [ ] ~~能否实现数量~~
- [x] 右上角的目标UI——需要Trigger实现
- [x] 敌人实现子弹攻击
- [x] 近战攻击需要动画
- [x] boss的攻击：状态+攻击
- [x] 机械臂的实现——还涉及到障碍物的实现（障碍物已实现）
- [x] 拼图的实现
- [x] 人物血量变化的尝试
- [ ] 灯光的摆放——人物自带光源+场景光源
- [ ] 音效

## 房间内调整
1. 灯光
2. 开局灯光

## 策划该注意的
1. 灯光
	- 在RoomDetail中的RoomStartTrigger中设置SetGlobalLightTrigger(全局光设置）；StartActiveTrigger为会在进入房间就调用的Trigger
	- 场景中的细节灯光为一开头就自己摆的
	- **场景中的物体的SpriteRenderer的Material应该使用Sprite-Lit-Default**，否则打不上光
2. 会碰撞的Collider都放在Envrionment Collider下面
	- 交互物体的collider只作trigger使用
3. 机械臂
	- 已经作为预制体放在Assets/Prefabs/Item下，人物拾取机械臂后便会装上；Trigger脚本为SetArmorFlag_Trigger(人物目前是没有机械臂动画，拾取后除了伤害增加没有变化）
4. 新增了自言自语ui的连贯性（仅自言自语）
	- 一个ActiveTrigger下放多个UIEasyShow，会变成根据按键进行下一句之类的（但就要注意ActiveTrigger的存放顺序）
5. 攻击
	- 人物按j键会进行攻击，一共四段（攻击后有一段时间会取消当前段数），能够攻击杀死小怪
6. 敌人放在Assets/Prefabs下
	- CloseAttack和FarAttack 近战和远程
	- 目前**近战**还有些问题
	- 现在已实现敌人如果存在房间里，人物是没法交互的
	- Boss boss敌人，目前的boss还没有死亡判定，也没有伤害判定
	- 受伤显示，目前小怪还没有受伤显示（shader还在研究中），但是会扣血死亡（boss还不会）
	- boss的掉落物品：也先不用搞

7. 音效
	- VoiceActive_Trigger能够播放音乐（一次），可作为拾取交互物品的时候播放
	- 如果每个房间需要切换背景音乐，先把上述trigger放在roomDetail下的RoomStartTrigger中
8. 数值调整
	- 小怪的数值调整：在小怪的NormalEnemyFSM组件上，用光标移到变量上会显示其中文介绍，自己调试看看
	- 人物数值调整：在PlayerController组件上，和小怪数值调整同理（光标blahblah）**目前玩家还不会死亡**
9. ui显示
	- 设置目标uiTrigger：SetTargetUI_;会将当前目标显示在目标ui上
	- 设置背包UITrigger：Bag_   ;会将记录的物品放入背包中；其目前的实现为会获取自身物体上的所有**SetConditionFlag**的目标，将目标标记为自身的使用者；有多少个目标，其就能使用多少次；**需要存储物品名字、图片（信息应该不用管）**
	- 拼图：拼图的详细实现见Assets/Scenes下的TestScene


## 新增事项
1.	场景中交互物品material调整为itemMaterial