CREATE OR REPLACE FUNCTION "identity"."get_user_details"
("user_id" VARCHAR)
RETURNS TABLE
(
    "id" VARCHAR,
    "bio" TEXT,
    "avatar" TEXT,
    "gender" INT8,
    "dateofbirth" DATE,          
    "level" INT8,
    "fullname" TEXT,             
    "gamesparticipatedcount" INT8,  
    "totalplaypal" INT8          
) AS $BODY$
BEGIN
    RETURN QUERY
    SELECT
        u."Id" AS id,
        u."Bio" AS bio,
        u."Avatar" AS avatar,
        CAST(u."Gender" AS BIGINT) AS gender,
        u."DateOfBirth" AS dateofbirth, -- Đổi tên cột
        CAST(u."Level" AS BIGINT) AS level,
        CONCAT(u."FirstName", ' ', u."LastName") AS fullname, -- Đổi tên cột
        COUNT(gp."GameId") AS gamesparticipatedcount, -- Đổi tên cột
        (SELECT COUNT(*)
        FROM "identity"."UserPlaypal" up
        WHERE up."UserId1" = user_id OR up."UserId2" = user_id) AS totalplaypal
    -- Đổi tên cột
    FROM
        "identity"."User" u
        LEFT JOIN
        "game"."GameParticipants" gp ON u."Id" = gp."UserId" AND gp."Status" = 2
    WHERE 
        u."Id" = user_id
    GROUP BY 
        u."Id", 
        u."FirstName", 
        u."LastName",
        u."Bio",
        u."Avatar",
        u."Gender",
        u."DateOfBirth",
        u."Level";
END;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100
ROWS 1000;