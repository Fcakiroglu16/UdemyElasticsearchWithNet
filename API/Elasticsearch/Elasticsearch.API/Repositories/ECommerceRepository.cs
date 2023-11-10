using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }


        public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerRequest)
        {
            #region Tip GÜVENSİZ Şekilde Search Yapma

            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(
            //   t => t.Field("customer_last_name.keyword").Value(customerFirstNameRequest))));

            #endregion

            #region Tip GÜVENLİ Şekilde Search Yapma

            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //.Query(q => q.Term(t => t.CustomerLastName.Suffix("keyword"),customerFirstNameRequest)));

            #endregion

            #region 2. YOL Query() i AYIRMA Tip GÜVENLİ Şekilde Search Yapma 

            var termQuery = new TermQuery("customer_last_name.keyword") { Value = customerRequest, CaseInsensitive = true }; // Burda arayacağımız datayı belirttik ve CaseInsensitive = true ile büyük/küçük harf duyarlılığını kapattık.

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(termQuery)));
            #endregion

            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            if (!result.IsValidResponse)
                return null;

            return result.Documents.ToImmutableList(); //dönen ham liste datası üzerinde değişiklik yapılmasın diye ToImmutableList() döndük, değişiklik yapılması çok önemli değilse ToList() de dönebilirdik.


        }


        public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> requestList)
        {

            List<FieldValue> terms = new List<FieldValue>();

            requestList.ForEach(x => //Burda aslında sadece tip dönüşümü yaptık. List<string> olan tipi List<FieldValue> ye dönüştürdük.
            {
                terms.Add(x);
            });


            #region 1.YOL 

            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(50) //50 tane data getir dedik.
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstName
            .Suffix("keyword"))
            .Terms(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in response.Hits)
                hit.Source.Id = hit.Id;
            #endregion

            #region 2. YOL Query() i AYIRMA Tip GÜVENLİ Şekilde Search Yapma 

            //var termsQuery = new TermsQuery() { Field = "customer_first_name.keyword", Terms = new TermsQueryField(terms.AsReadOnly()) };
            //var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));
            //foreach (var hit in response.Hits)
            //    hit.Source.Id = hit.Id;
            #endregion


            return response.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .Prefix(p => p
            .Field(f => f.CustomerFullName
            .Suffix("keyword")).Value(request))));

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .Range(r => r
            .NumberRange(nr => nr
            .Field(f => f.Taxful_Total_Price)
            .Gte(fromPrice).Lte(toPrice))))); // ToPrice dan düşük, FromPrice dan yüksek olucak

            return response.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> MatchAllAsync()
        {
            var response = await _client.SearchAsync<ECommerce>(s => s
            .Size(100).Index(indexName).Query(q => q.MatchAll()));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page, int pageSize)
        {

            //page=1, pageSize=10 => 1-10
            //page=2, pageSize=10 => 11-20
            //page=3, pageSize=10 => 21-30

            var pageFrom = (page - 1) * 10;

            var response = await _client.SearchAsync<ECommerce>(q => q
            .Size(pageSize).From(pageFrom)  //Her sayfada 10 data olacak şekilde Sayfalama(Pagination) yapıyoruz.
            .Index(indexName)
            .Query(q => q.MatchAll()));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }


            return response.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> WildCardQuery(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s
            .Size(100)
            .Index(indexName)
            .Query(q => q
            .Wildcard(w => w
            .Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(request))));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q
            .Fuzzy(fu => fu
            .Field(f => f.CustomerFirstName
            .Suffix("keyword")).Value(request)
            .Fuzziness(new Fuzziness(2)))) //Aranan kelimenin herhangi bir yerinde 2 harf hatalı olabilir ya da olamayabilir.
            .Sort(sort => sort
            .Field(f => f.Taxful_Total_Price, new FieldSort() { Order = SortOrder.Desc }))); //Gelen datayı tutar değerine göre büyükten küçüğe doğru sıraladık.
            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s
            .Index(indexName)
            .Size(150)
            .Query(q => q
            .Match(m => m
            .Field(f => f.Category)
            //.Query(request)))); //Artık .Suffix("keyword") demiyoruz çünkü TermQuery değil FullText Query yapıyoruz. Birde bu şekilde OR şeklinde arar. Sen operatorde AND diye belirtmezsen default OR diye arar.
            .Query(request).Operator(Operator.And)))); //Artık Operatorumuzu AND diye belirttik Meselam Musa Yılmaz diye arama yaptığında SADECE adı Musa VE/AND soyadı Yılmaz olanları getirecek.
            
            
            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> MultiMatchQueryFullTextAsync(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q
            .MultiMatch(mm => mm
            .Fields(new Field("customer_first_name")
            .And(new Field("customer_last_name"))
            .And(new Field("customer_full_name")))
            .Query(request))));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

             return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .MatchBoolPrefix(m => m
            .Field(f => f.CustomerFullName)
            .Query(request)
            .Operator(Operator.Or)))); ; // YA DA/OR Operatorü ile ARA DEDİK.

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync( string request)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .MatchPhrase(m => m
            .Field(f => f.CustomerFullName)
            .Query(request))));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double taxtfulTotalPrice, string categoryName,string manufacturer)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName) //Birden fazla alanda/field de Bileşik(birden fazla data ile) arama yapma
            .Size(100)
            .Query(q => q
            .Bool(b => b
                .Must(m => m
                    .Term(t => t
                        .Field("geoip.city_name")
                        .Value(cityName)))
                .MustNot(mn => mn
                    .Range(r => r
                        .NumberRange(nr => nr
                            .Field(f => f.Taxful_Total_Price)
                            .Lte(taxtfulTotalPrice))))
                .Should(s => s
                    .Term(t =>t
                        .Field(f => f.Category.Suffix("keyword"))
                        .   Value(categoryName)))
                .Filter(f => f
                    .Term(t =>t
                        .Field("manufacturer.keyword")
                        .Value(manufacturer))))
            ));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList() ;
        }


        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName)
        {
            var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName) //Bir alanda/field de Bileşik(birden fazla) arama yapma
            .Size(100)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Match(m => m
                            .Field(f => f.CustomerFullName).Query(customerFullName))))

            )); ;

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToImmutableList();
        }




    }
}


// public int Boy {  get; init; } => set yerine init dediğimizde Boy'a bir değer verdikten sonra bir daha değiştiremezsin.