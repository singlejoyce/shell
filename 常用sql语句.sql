DROP TABLE IF EXISTS nsytroledb.bug_role_vip_20170821;
create table nsytroledb.bug_role_vip_20170821
(
  account int unsigned,
  roleid int unsigned,
	vip_level int unsigned,
	vip_expired_time int unsigned
);


insert into nsytroledb.bug_role_vip_20170821 
SELECT nsytroledb.role.account,nsytroledb.role.roleid,nsytroledb.vip.`level`,nsytroledb.vip.expired_time
from nsytroledb.vip
LEFT JOIN nsytroledb.role
on nsytroledb.role.roleid=nsytroledb.vip.roleid where nsytroledb.vip.`level` >12 and nsytroledb.vip.expired_time < UNIX_TIMESTAMP('2017-8-20 00:00:00');


#account-role表拼接
SELECT nsytaccountdb.player.account,nsytaccountdb.player.username,nsytroledb.role.roleid,nsytroledb.role.rolename,nsytroledb.role.sex,nsytroledb.role.`level`
from nsytaccountdb.player 
LEFT JOIN nsytroledb.role 
on nsytaccountdb.player.account=role.account 
where username LIKE "testy%"  and pid=1 ORDER BY account DESC

#role-dancegroup拼接查询
SELECT nsytroledb.dance_group.group_id,nsytroledb.dance_group.group_name,nsytroledb.dance_group.leader_id,nsytroledb.role.rolename
from nsytroledb.role
LEFT JOIN nsytroledb.dance_group
on nsytroledb.role.roleid=nsytroledb.dance_group.leader_id
where nsytroledb.role.roleid=nsytroledb.dance_group.leader_id;


#starshow-role表拼接
SELECT starshow_cloth.roleid,role.rolename,role.sex,starshow_cloth.score 
from starshow_cloth 
LEFT JOIN role 
on starshow_cloth.roleid=role.roleid 
where starshow_cloth.period=28 and role.sex=2
ORDER BY score DESC
LIMIT 100

#T台秀报名查询
SELECT nsytroledb.dance_group.group_id,nsytroledb.dance_group.group_name,nsytroledb.dance_group.leader_id,
nsytroledb.tshow_score.win,nsytroledb.tshow_score.winstreak,nsytroledb.tshow_score.lastorder,nsytroledb.tshow_score.rolejoin
from nsytroledb.tshow_score
LEFT JOIN nsytroledb.dance_group
on nsytroledb.tshow_score.groupid=nsytroledb.dance_group.group_id
where nsytroledb.tshow_score.groupid=nsytroledb.dance_group.group_id;


#查询机器人创建的舞团id
select dance_group_id, count(roleid) from role where rolename like 'Robot%' and dance_group_id > 0 group by dance_group_id;

#多个查询条件
select * from clothgroup WHERE roleid in (SELECT roleid from role WHERE account in (SELECT account from nsytaccountdb.player WHERE username LIKE "testy0%" ) )


#跨表查询
SELECT * from role where account in (SELECT account from nsytaccountdb.player WHERE username LIKE "testy%" AND pid=1 )


#清空所有机器人舞团信息
delete from dance_group where group_id in (select distinct(dance_group_id) from role where rolename like 'Robot%' and dance_group_id > 0);
delete from dance_group_member where group_id in (select distinct(dance_group_id) from role where rolename like 'Robot%' and dance_group_id > 0);
update role set dance_group_id = 0 where rolename like 'Robot%' and dance_group_id > 0;

#删除机器人账号和角色
delete from nsytaccountdb.player where account in (select account from role where rolename like 'Robot%');
delete from role where roleid in (select * from (select b.roleid from role b where b.rolename like 'Robot%') as tmp);

#时间函数
SELECT * from nsytroledb.mail where receiverid= 2 and sendtime> UNIX_TIMESTAMP('2017-04-04 00:00:00');