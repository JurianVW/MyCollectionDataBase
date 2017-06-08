using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace Repository.Data
{
    public class CollectionSQLContext : ICollectionContext
    {
        private DatabaseConnection con = new DatabaseConnection();

        public bool CheckMyItem(int itemID, int userID)
        {
            string query = "SELECT * FROM [Item] i " +
                           "JOIN List_Item li ON li.Item_ID = i.ID " +
                           "JOIN List l ON l.ID = li.List_ID " +
                           "WHERE l.User_ID = @User_ID AND i.ID = @ID ";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", itemID);
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Item GetItem(int itemID, int userID)
        {
            string query = "SELECT i.*, b.*, c.Type AS 'CaseType', c.Cover, m.Type AS 'MediaType', m.Runtime, m.Platform FROM [Item] i " +
                           "JOIN List_Item li ON li.Item_ID = i.ID " +
                           "JOIN List l ON l.ID = li.List_ID " +
                           "LEFT OUTER JOIN [Book] b ON b.Item_ID = i.ID " +
                           "LEFT OUTER JOIN [Case] c ON c.Item_ID = i.ID " +
                           "LEFT OUTER JOIN [Media] m ON m.Item_ID = i.ID " +
                           "WHERE l.User_ID = @User_ID AND i.ID = @ID ";
            Item item = new Item();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", itemID);
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        item = new Item
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            ItemType = dataReader.GetString(dataReader.GetOrdinal("Type")),
                            Title = dataReader.GetString(dataReader.GetOrdinal("Title")),
                            Description = dataReader.IsDBNull(dataReader.GetOrdinal("Description")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Description")),
                            Price = dataReader.IsDBNull(dataReader.GetOrdinal("Price")) ? 0 : dataReader.GetDecimal(dataReader.GetOrdinal("Price")),
                            Year = dataReader.IsDBNull(dataReader.GetOrdinal("Year")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Year")),
                            Country = dataReader.IsDBNull(dataReader.GetOrdinal("Country")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Country")),
                            Retailer = dataReader.IsDBNull(dataReader.GetOrdinal("Retailer")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Retailer")),
                            Exclusive = dataReader.IsDBNull(dataReader.GetOrdinal("Exclusive")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Exclusive")),
                            Limited = dataReader.IsDBNull(dataReader.GetOrdinal("Limited")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Limited"))
                        };
                        switch (item.ItemType)
                        {
                            case "Book":
                                item.ItemBook = new Book();
                                item.ItemBook.Format = dataReader.IsDBNull(dataReader.GetOrdinal("Format")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Format"));
                                item.ItemBook.Language = dataReader.IsDBNull(dataReader.GetOrdinal("Language")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Language"));
                                item.ItemBook.Pages = dataReader.IsDBNull(dataReader.GetOrdinal("Pages")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Pages"));
                                break;

                            case "Case":
                                item.ItemCase = new Case();
                                item.ItemCase.CaseType = dataReader.GetString(dataReader.GetOrdinal("CaseType"));
                                item.ItemCase.Cover = dataReader.IsDBNull(dataReader.GetOrdinal("Cover")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Cover"));
                                break;

                            case "Media":
                                item.ItemMedia = new Media();
                                item.ItemMedia.MediaType = dataReader.GetString(dataReader.GetOrdinal("MediaType"));
                                item.ItemMedia.Runtime = dataReader.IsDBNull(dataReader.GetOrdinal("Runtime")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Runtime"));
                                item.ItemMedia.Platform = dataReader.IsDBNull(dataReader.GetOrdinal("Platform")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Platform"));
                                break;
                        }
                    }
                }
            }
            GetItemTags(item);
            GetImages(item);
            return item;
        }

        public List<Item> GetItems(int userID)
        {
            string query = "SELECT * FROM [Item] i JOIN List_Item li ON li.Item_ID = i.ID JOIN List l ON l.ID = li.List_ID WHERE l.User_ID = @User_ID";

            List<Item> items = new List<Item>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Item item = new Item
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            ItemType = dataReader.GetString(dataReader.GetOrdinal("Type")),
                            Title = dataReader.GetString(dataReader.GetOrdinal("Title")),
                            Description = dataReader.IsDBNull(dataReader.GetOrdinal("Description")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Description")),
                            Price = dataReader.IsDBNull(dataReader.GetOrdinal("Price")) ? 0 : dataReader.GetDecimal(dataReader.GetOrdinal("Price")),
                            Year = dataReader.IsDBNull(dataReader.GetOrdinal("Year")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Year")),
                            Country = dataReader.IsDBNull(dataReader.GetOrdinal("Country")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Country")),
                            Retailer = dataReader.IsDBNull(dataReader.GetOrdinal("Retailer")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Retailer")),
                            Exclusive = dataReader.IsDBNull(dataReader.GetOrdinal("Exclusive")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Exclusive")),
                            Limited = dataReader.IsDBNull(dataReader.GetOrdinal("Limited")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Limited"))
                        };
                        GetItemTags(item);
                        GetImages(item);
                        items.Add(item);
                    }
                }
            }
            return items;
        }

        public List<Item> GetItems(int listID, int userID)
        {
            string query = "SELECT i.*  FROM [Item] i JOIN List_Item li ON li.Item_ID = i.ID JOIN List l ON l.ID = li.List_ID WHERE li.List_ID = @List_ID AND l.User_ID = @User_ID";
            List<Item> items = new List<Item>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@List_ID", listID);
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Item item = new Item
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            ItemType = dataReader.GetString(dataReader.GetOrdinal("Type")),
                            Title = dataReader.GetString(dataReader.GetOrdinal("Title")),
                            Description = dataReader.IsDBNull(dataReader.GetOrdinal("Description")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Description")),
                            Price = dataReader.IsDBNull(dataReader.GetOrdinal("Price")) ? 0 : dataReader.GetDecimal(dataReader.GetOrdinal("Price")),
                            Year = dataReader.IsDBNull(dataReader.GetOrdinal("Year")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Year")),
                            Country = dataReader.IsDBNull(dataReader.GetOrdinal("Country")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Country")),
                            Retailer = dataReader.IsDBNull(dataReader.GetOrdinal("Retailer")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Retailer")),
                            Exclusive = dataReader.IsDBNull(dataReader.GetOrdinal("Exclusive")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Exclusive")),
                            Limited = dataReader.IsDBNull(dataReader.GetOrdinal("Limited")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Limited"))
                        };
                        GetItemTags(item);
                        GetImages(item);
                        items.Add(item);
                    }
                }
            }
            return items;
        }

        public void SaveListItem(Item item)
        {
            string query = "INSERT INTO [List_Item] ([List_ID], [Item_ID])" +
                           "VALUES (@ListID, @ItemID)";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ListID", item.ListID);
                command.Parameters.AddWithValue("@ItemID", item.ID);
                command.ExecuteNonQuery();
            }
        }

        public void SaveItem(Item item)
        {
            string query = item.ID == 0
                ? "INSERT INTO [Item] ([Type], [Title], [Description], [Price], [Year], [Country], [Retailer], [Exclusive], [Limited])" +
                  " OUTPUT INSERTED.ID" +
                  " VALUES (@Type, @Title, @Description, @Price, @Year, @Country, @Retailer, @Exclusive, @Limited)"
                : "UPDATE [Item] SET [Type]=@Type, [Title]=@Title, [Description]=@Description, [Price]=@Price, [Year]=@Year, [Country]=@Country, [Retailer]=@Retailer, [Exclusive]=@Exclusive, [Limited]=@Limited" +
                  " WHERE ID=@ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Type", item.ItemType);
                command.Parameters.AddWithValue("@Title", item.Title);
                if (item.Description.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Description", item.Description);
                }
                else
                {
                    command.Parameters.AddWithValue("@Description", DBNull.Value);
                }
                if (item.Price != 0)
                {
                    command.Parameters.AddWithValue("@Price", item.Price);
                }
                else
                {
                    command.Parameters.AddWithValue("@Price", DBNull.Value);
                }
                if (item.Year != 0)
                {
                    command.Parameters.AddWithValue("@Year", item.Year);
                }
                else
                {
                    command.Parameters.AddWithValue("@Year", DBNull.Value);
                }
                if (item.Country.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Country", item.Country);
                }
                else
                {
                    command.Parameters.AddWithValue("@Country", DBNull.Value);
                }
                if (item.Retailer.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Retailer", item.Retailer);
                }
                else
                {
                    command.Parameters.AddWithValue("@Retailer", DBNull.Value);
                }
                if (item.Exclusive.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Exclusive", item.Retailer);
                }
                else
                {
                    command.Parameters.AddWithValue("@Exclusive", DBNull.Value);
                }
                if (item.Limited != 0)
                {
                    command.Parameters.AddWithValue("@Limited", item.Retailer);
                }
                else
                {
                    command.Parameters.AddWithValue("@Limited", DBNull.Value);
                }

                if (item.ID != 0)
                {
                    command.Parameters.AddWithValue("@ID", item.ID);
                }

                int result = Convert.ToInt32(command.ExecuteScalar());
                if (item.ID == 0)
                {
                    item.ID = result;
                    SaveListItem(item);
                }
                SaveItemTags(item);
                SaveImages(item);
            }
        }

        public void SaveItem(Item item, Book itemBook)
        {
            SaveItem(item);
            string query = item.ID == 0
                ? "INSERT INTO [Book] ([Item_ID], [Format], [Language], [Pages])" +
                  " VALUES (@Item_ID, @Format, @Language, @Pages)"
                : "UPDATE [Book] SET [Item_ID]=@Item_ID, [Format]=@Format, [Language]=@Language, [Pages]=@Pages" +
                  " WHERE Item_ID=@Item_ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Item_ID", item.ID);

                if (itemBook.Format.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Format", itemBook.Format);
                }
                else
                {
                    command.Parameters.AddWithValue("@Format", DBNull.Value);
                }
                if (itemBook.Language.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Language", itemBook.Language);
                }
                else
                {
                    command.Parameters.AddWithValue("@Language", DBNull.Value);
                }
                if (itemBook.Pages != 0)
                {
                    command.Parameters.AddWithValue("@Pages", itemBook.Pages);
                }
                else
                {
                    command.Parameters.AddWithValue("@Pages", DBNull.Value);
                }

                command.ExecuteNonQuery();
            }
        }

        public void SaveItem(Item item, Case itemCase)
        {
            SaveItem(item);
            string query = item.ID == 0
                ? "INSERT INTO [Case] ([Item_ID], [Type], [Cover])" +
                  " VALUES (@Item_ID, @Type, @Cover)"
                : "UPDATE [Case] SET [Item_ID]=@Item_ID, [Type]=@Type, [Cover]=@Cover" +
                  " WHERE Item_ID=@Item_ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Item_ID", item.ID);
                command.Parameters.AddWithValue("@Type", itemCase.CaseType);

                if (itemCase.Cover.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Cover", itemCase.Cover);
                }
                else
                {
                    command.Parameters.AddWithValue("@Cover", DBNull.Value);
                }

                command.ExecuteNonQuery();
            }
            SaveDiscs(item);
        }

        public void SaveItem(Item item, Media itemMedia)
        {
            int oldItemID = item.ID;
            SaveItem(item);
            string query = item.ID != oldItemID
                ? "INSERT INTO [Media] ([Item_ID], [Type], [Runtime], [Platform])" +
                  " VALUES (@Item_ID, @Type, @Runtime, @Platform)"
                : "UPDATE [Media] SET [Item_ID]=@Item_ID, [Type]=@Type, [Runtime]=@Runtime, [Platform]=@Platform" +
                  " WHERE Item_ID=@Item_ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Item_ID", item.ID);
                command.Parameters.AddWithValue("@Type", itemMedia.MediaType);
                if (itemMedia.Runtime != 0)
                {
                    command.Parameters.AddWithValue("@Runtime", itemMedia.Runtime);
                }
                else
                {
                    command.Parameters.AddWithValue("@Runtime", DBNull.Value);
                }
                if (itemMedia.Platform.Trim() != "")
                {
                    command.Parameters.AddWithValue("@Platform", itemMedia.Platform);
                }
                else
                {
                    command.Parameters.AddWithValue("@Platform", DBNull.Value);
                }

                command.ExecuteNonQuery();
            }
            SaveDiscs(item);
        }

        public void SaveItemTags(Item item)
        {
            List<Tag> existingTags = new List<Tag>();

            string query = "SELECT * FROM [Tag] WHERE [Type] = 'Tag' OR [Type] = 'Genre'";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        existingTags.Add(new Tag
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            Name = dataReader.GetString(dataReader.GetOrdinal("Name")),
                            Type = dataReader.GetString(dataReader.GetOrdinal("Type")),
                        });
                    }
                }
            }

            foreach (Tag t in item.tags)
            {
                bool exists = false;
                foreach (Tag et in existingTags)
                {
                    if (et.Name == t.Name && et.Type == t.Type)
                    {
                        exists = true;
                        t.ID = et.ID;
                    }
                }
                if (!exists)
                {
                    CreateTag(t);
                }
                SaveTag(t, item.ID);
            }
        }

        public void CreateTag(Tag tag)
        {
            string query = "INSERT INTO [Tag] ([Name], [Type])" +
                           " OUTPUT INSERTED.ID" +
                           " VALUES (@Name, @Type)";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Name", tag.Name);
                command.Parameters.AddWithValue("@Type", tag.Type);
                tag.ID = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void SaveTag(Tag tag, int itemID)
        {
            string query = "INSERT INTO [Item_Tag] ([Item_ID], [Tag_ID])" +
                          " VALUES (@ItemID, @TagID)";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ItemID", itemID);
                command.Parameters.AddWithValue("@TagID", tag.ID);
                command.ExecuteNonQuery();
            }
        }

        public void GetItemTags(Item item)
        {
            string query = "SELECT * FROM TAG t " +
                           "JOIN[Item_Tag] it on it.Tag_ID = t.ID " +
                           "WHERE it.Item_ID = @ItemID";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ItemID", item.ID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        item.tags.Add(new Tag()
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            Name = dataReader.GetString(dataReader.GetOrdinal("Name")),
                            Type = dataReader.GetString(dataReader.GetOrdinal("Type"))
                        });
                    }
                }
            }
        }

        public void SaveImages(Item item)
        {
            string query = "INSERT INTO [Image] ([Item_ID], [Item_Picture], [Position])" +
                           " VALUES (@ItemID, @ItemPicture, @Position)";

            foreach (Image i in item.images)
            {
                using (SqlCommand command = con.Connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@ItemID", item.ID);
                    command.Parameters.AddWithValue("@ItemPicture", i.ItemPicture);
                    command.Parameters.AddWithValue("@Position", i.Position);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void GetImages(Item item)
        {
            string query = "SELECT * FROM [Image] i " +
                           "WHERE i.Item_ID = @ItemID";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ItemID", item.ID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        item.images.Add(new Image
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            ItemPicture = dataReader.GetString(dataReader.GetOrdinal("Item_Picture")),
                            Position = dataReader.GetInt32(dataReader.GetOrdinal("Position"))
                        });
                    }
                }
            }
        }

        public void SaveDiscs(Item item)
        {
            List<Disc> discsDB = GetDiscs();
            List<Disc> discs = new List<Disc>();

            if (item.ItemType == "Case")
            {
                discs = item.ItemCase.Discs;
            }
            else if (item.ItemType == "Media")
            {
                discs = item.ItemMedia.Discs;
            }

            foreach (Disc d in discs)
            {
                foreach (Disc disc in discsDB)
                {
                    if (d.Format == disc.Format)
                    {
                        d.ID = disc.ID;
                    }
                }
                for (int i = 0; i < d.Amount; i++)
                {
                    string query = "INSERT INTO [Case_Media_Disc] ([Disc_ID], [Case_ID], [Media_ID])" +
                                   " VALUES (@DiscID, @CaseID, @MediaID)";
                    using (SqlCommand command = con.Connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@DiscID", d.ID);
                        if (item.ItemType == "Case") { command.Parameters.AddWithValue("@CaseID", item.ID); } else { command.Parameters.AddWithValue("@CaseID", DBNull.Value); }
                        if (item.ItemType == "Media") { command.Parameters.AddWithValue("@MediaID", item.ID); } else { command.Parameters.AddWithValue("@MediaID", DBNull.Value); }
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteItem(int itemID)
        {
            string query = "DELETE FROM Item WHERE ID=@ItemID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ItemID", itemID);
                command.ExecuteNonQuery();
            }
        }

        public List GetList(int listID, int userID)
        {
            string query = "SELECT * FROM [List] WHERE ID = @ID AND User_ID = @User_ID";
            List list = new List();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", listID);
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        list = new List
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            UserID = dataReader.GetInt32(dataReader.GetOrdinal("User_ID")),
                            Name = dataReader.GetString(dataReader.GetOrdinal("Name")),
                            items = new List<Item>(GetItems(listID, userID))
                        };
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("Description")))
                        {
                            list.Description = dataReader.GetString(dataReader.GetOrdinal("Description"));
                        }
                    }
                }
            }
            return list;
        }

        public List<List> GetLists(int userID)
        {
            string query = "SELECT * FROM [List] WHERE User_ID = @User_ID";
            List<List> lists = new List<List>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@User_ID", userID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        List list = new List
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            UserID = dataReader.GetInt32(dataReader.GetOrdinal("User_ID")),
                            Name = dataReader.GetString(dataReader.GetOrdinal("Name")),
                        };
                        list.items = new List<Item>(GetItems(list.ID, userID));
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("Description")))
                        {
                            list.Description = dataReader.GetString(dataReader.GetOrdinal("Description"));
                        }
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        public void SaveList(List list, int userID)
        {
            string query = list.ID == 0
                ? "INSERT INTO [List] (User_ID, Name, Description)" +
                  " OUTPUT INSERTED.ID" +
                  " VALUES (@User_ID, @Name, @Description)"
                : "UPDATE [List] SET Name=@Name, Description=@Description" +
                  " WHERE ID=@ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Name", list.Name);
                command.Parameters.AddWithValue("@Description", list.Description == "" ? "" : list.Description);
                command.Parameters.AddWithValue("@User_ID", userID);

                if (list.ID != 0)
                {
                    command.Parameters.AddWithValue("@ID", list.ID);
                }

                int result = Convert.ToInt32(command.ExecuteScalar());
                if (list.ID == 0)
                {
                    list.ID = result;
                }
            }
        }

        public void DeleteList(int listID)
        {
            string query = "spDeleteListAndItems";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ListID", listID);
                command.ExecuteNonQuery();
            }
        }

        public List<Disc> GetDiscs()
        {
            string query = "SELECT * FROM [Disc]";
            List<Disc> discs = new List<Disc>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        discs.Add(new Disc
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            Format = dataReader.GetString(dataReader.GetOrdinal("Format"))
                        });
                    }
                }
            }
            return discs;
        }

        public List<string> GetFinishes()
        {
            string query = "SELECT [Name] FROM [Tag] WHERE [Type] = 'Finish'";
            List<string> finishes = new List<string>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        finishes.Add(dataReader.GetString(dataReader.GetOrdinal("Name")));
                    }
                }
            }
            return finishes;
        }

        public List<string> GetGenres()
        {
            string query = "SELECT [Name] FROM [Tag] WHERE [Type] = 'Genre'";
            List<string> genres = new List<string>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        genres.Add(dataReader.GetString(dataReader.GetOrdinal("Name")));
                    }
                }
            }
            return genres;
        }
    }
}