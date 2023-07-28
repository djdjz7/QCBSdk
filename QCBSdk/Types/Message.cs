using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBSdk.Types
{
    public class Message
    {
        public string Id { get; set; }
        public string ChannelId { get; set; }
        public string GuildId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime EditedTimestamp { get; set; }
        public bool MentionEveryone { get; set; }
        public User Author { get; set; }
        public MessageAttachment[] Attachments { get; set; }

    }

    public class MessageAttachment
    {
        public string Url { get; set; }
    }
    public class MessageEmbed
    {
        public string Title { get; set; }
        public string Prompt { get; set; }
        public MessageEmbedThumbnail Thumbnail { get; set; }
        public MessageEmbedField[] Fields { get; set; }
    }

    public class MessageEmbedField
    {
        public string Name { get; set; }
    }

    public class MessageEmbedThumbnail
    {
        public string Url { get; set; }
    }

    public class MessageArk
    {
        public int TemplateId { get; set; }
        public MessageArkKv[] kv { get; set; }
    }

    public class MessageArkKv
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public MessageArkObj[] Obj { get; set; }
    }

    public class MessageArkObj
    {
        public MessageArkObjKv[] ObjKv { get; set; }
    }
    public class MessageArkObjKv
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class MessageReference
    {
        public string MessageId { get; set; }
        public bool IgnoreGetMessageError { get; set; }
    }
    public class MessageMarkdown
    {
        public int TemplateId { get; set; }
        public string CustomTemplateId { get; set; }
        public MessageMarkdownParams Params { get; set; }
        public string Content { get; set; }
    }

    public class MessageMarkdownParams
    {
        public string Key { get; set; }
        public string[] Values { get; set; }
    }

    public class MessageDelete
    {
        public Message Message { get; set; }
        public User OpUser { get; set; }
    }

    /*
    public class MessageKeyboard
    {
        public string Id { get; set; }
        public InlineKeyboard Content { get; set; }
    }*/
}
