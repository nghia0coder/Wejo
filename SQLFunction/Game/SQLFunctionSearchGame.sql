CREATE OR REPLACE FUNCTION game.search_games(
    p_longitude DOUBLE PRECISION,
    p_latitude DOUBLE PRECISION,
    p_sport_id INTEGER,
    p_start_time TIMESTAMP,
    p_skill_start INTEGER,
    p_skill_end INTEGER,
    p_page_size INTEGER,
    p_offset INTEGER
)
RETURNS TABLE (
    "Id" UUID,
    "SportId" INTEGER,
    "CreatedBy" TEXT,
    "SportFormatId" INTEGER,
    "VenueId" UUID,
    "GameTypeId" INTEGER,
    "Area" TEXT,
    "StartTime" TIMESTAMP,
    "EndTime" TIMESTAMP,
    "GameAccess" BOOLEAN,
    "GameSkill" BOOLEAN,
    "SkillStart" INTEGER,
    "SkillEnd" INTEGER,
    "TotalPlayer" INTEGER,
    "CurrentPlayer" INTEGER,
    "SlotLeft" INTEGER,
    "Description" TEXT,
    "Location" GEOMETRY,
    "Distance" DOUBLE PRECISION,
    "PlayerAvatarJson" JSONB  -- ✅ Change from TEXT[] to JSONB
)
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        g."Id",
        g."SportId",
        g."CreatedBy"::TEXT,  
        g."SportFormatId",
        g."VenueId",
        g."GameTypeId",
        g."Area"::TEXT,        
        g."StartTime",
        g."EndTime",
        g."GameAccess",
        g."GameSkill",
        g."SkillStart",
        g."SkillEnd",
        g."TotalPlayer",

        -- ✅ Explicitly cast COUNT(*) to INTEGER
        COALESCE((
            SELECT COUNT(*)::INTEGER 
            FROM game."GameParticipants" gp 
            WHERE gp."GameId" = g."Id"
        ), 0) AS "CurrentPlayer",

        -- ✅ Explicitly cast SlotLeft calculation to INTEGER
        (g."TotalPlayer" - COALESCE((
            SELECT COUNT(*)::INTEGER 
            FROM game."GameParticipants" gp 
            WHERE gp."GameId" = g."Id"
        ), 0))::INTEGER AS "SlotLeft",

        g."Description"::TEXT, 
        g."Location",
        ST_Distance(g."Location", ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326)) AS "Distance",

        -- 🆕 Convert TEXT[] to JSONB
        COALESCE(
            (
                SELECT jsonb_agg(u."Avatar") 
                FROM game."GameParticipants" gp
                JOIN identity."User" u ON gp."UserId" = u."Id"
                WHERE gp."GameId" = g."Id"
            ), '[]'::jsonb
        ) AS "PlayerAvatarJson"
        
    FROM game."Game" g 
    WHERE g."SportId" = p_sport_id
      AND g."Status" = 0
      AND (p_start_time IS NULL OR g."StartTime" >= p_start_time)
      AND (p_skill_start IS NULL OR g."SkillStart" >= p_skill_start)
      AND (p_skill_end IS NULL OR g."SkillEnd" <= p_skill_end)
    ORDER BY "Distance"
    LIMIT p_page_size OFFSET p_offset;
END;
$$ LANGUAGE plpgsql;
