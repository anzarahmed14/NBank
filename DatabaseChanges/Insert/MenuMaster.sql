
INSERT INTO dbo.MenuMaster
(
    MenuName,
    DisplayName,
    IsActive,
    AllowCreate,
    AllowView,
    AllowDelete,
    AllowEdit,
    AllowPrint
)
VALUES
-- 1️⃣ Company Group Master
(
    'MenuCompanyGroup',
    'Company Group',
    1,  -- IsActive
    0,  -- AllowCreate
    0,  -- AllowView
    0,  -- AllowDelete
    0,  -- AllowEdit
    0   -- AllowPrint
),

-- 2️⃣ Map Company Group
(
    'MenuMapCompanyGroup',
    'Map Company Group',
    1,
    0,
    0,
    0,
    0,
    0
),

-- 3️⃣ Map User Company Group
(
    'MenuMapUserCompanyGroup',
    'Map User Company Group',
    1,
    0,
    0,
    0,
    0,
    0
);
GO
