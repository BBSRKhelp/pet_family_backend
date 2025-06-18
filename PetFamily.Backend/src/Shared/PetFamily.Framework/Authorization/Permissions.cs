namespace PetFamily.Framework.Authorization;

public static class Permissions
{
    public static class Volunteers
    {
        public const string CREATE = "volunteers.create";
        public const string GET = "volunteers.get";
        public const string UPDATE_MAIN_INFO = "volunteers.update.main_info";
        public const string DELETE = "volunteers.delete";

        public const string PET_ADD = "volunteers.pets.add";
        public const string PET_FILES_UPLOAD = "volunteers.pets.files.upload";
        public const string PET_MAIN_PHOTO_SET = "volunteers.pets.main_photo.set";
        public const string PET_POSITION_CHANGE = "volunteers.pets.position.change";
        public const string PET_MAIN_INFO_UPDATE = "volunteers.pets.main_info.update";
        public const string PET_STATUS_UPDATE = "volunteers.pets.status.update";
        public const string PET_SOFT_DELETE = "volunteers.pets.soft_delete";
        public const string PET_HARD_DELETE = "volunteers.pets.hard_delete";
    }

    public static class Pets
    {
        public const string GET = "pets.get";
    }

    public static class Species
    {
        public const string CREATE = "species.create";
        public const string GET = "species.get";
        public const string DELETE = "species.delete";

        public const string BREED_ADD = "species.breeds.add";
        public const string BREED_DELETE = "species.breeds.delete";
    }

    public static class Breeds
    {
        public const string GET = "breeds.get";
    }
}