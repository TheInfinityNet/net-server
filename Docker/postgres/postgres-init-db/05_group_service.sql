\c group_service_db
--
-- PostgreSQL database dump
--

-- Dumped from database version 12.20 (Debian 12.20-1.pgdg110+1)
-- Dumped by pg_dump version 12.20

-- Started on 2024-10-26 17:21:30 UTC

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 16470)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 2980 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 16891)
-- Name: groups; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.groups (
    id uuid NOT NULL,
    name character varying(255) NOT NULL,
    description text,
    owner_id uuid NOT NULL,
    created_by uuid,
    created_at timestamp without time zone NOT NULL,
    updated_by uuid,
    updated_at timestamp without time zone,
    deleted_by uuid,
    deleted_at timestamp without time zone,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.groups OWNER TO "infinitynetUser";

--
-- TOC entry 204 (class 1259 OID 16899)
-- Name: groups_members; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.groups_members (
    id uuid NOT NULL,
    role integer NOT NULL,
    group_id uuid NOT NULL,
    user_profile_id uuid NOT NULL,
    created_by uuid,
    created_at timestamp without time zone NOT NULL,
    updated_by uuid,
    updated_at timestamp without time zone,
    deleted_by uuid,
    deleted_at timestamp without time zone,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.groups_members OWNER TO "infinitynetUser";

--
-- TOC entry 2973 (class 0 OID 16891)
-- Dependencies: 203
-- Data for Name: groups; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.groups (id, name, description, owner_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
02d11521-1d02-4af8-b31a-74fa8bb08778	Libero soluta commodi.	Quia dolorem nam quo itaque ad quae voluptatem quos illum ab et fugiat fugiat quia ut voluptas porro aliquid facere soluta molestiae esse sequi laborum nostrum error tenetur veniam omnis et in ut voluptatem eum necessitatibus quia est commodi vel nihil esse voluptatem modi maxime modi consequatur fugiat dolorum atque.	134e6153-f93b-4592-8bd7-ae30e9321017	134e6153-f93b-4592-8bd7-ae30e9321017	2024-10-27 00:14:34.374302	\N	\N	\N	\N	f
06314656-9772-4193-b105-edd0c136d72f	Voluptatibus fugiat recusandae.	Architecto maxime debitis sapiente eius et tempora ut aspernatur sunt ad dolorem perferendis consequuntur non qui eum dolor ducimus omnis suscipit quia eos harum nam sit soluta debitis quia et est a illo earum qui veritatis molestiae ullam non non sed totam atque aliquid quos ratione est in non omnis.	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.364072	\N	\N	\N	\N	f
0f087e27-7a21-45ea-b6d3-7296c5360cc3	Eum sed quod.	Et optio consequatur dolorem assumenda velit quos minima reprehenderit deserunt voluptas aperiam iusto laudantium quas eum velit esse corporis perspiciatis iure nesciunt rerum ea et quibusdam minus ut cupiditate blanditiis eos corrupti velit nisi nam omnis harum hic consectetur dolorum libero soluta molestiae ut occaecati consequatur consequatur ducimus qui facilis.	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	2024-10-27 00:14:34.363552	\N	\N	\N	\N	f
1080d248-b0ca-4124-accd-2ba2f0d30cba	Ex quia eveniet.	Ratione quia corrupti omnis ut qui in et repellat quo est placeat sint veniam minus beatae et ad unde optio amet corrupti perspiciatis fuga inventore veritatis id ab voluptatem quam dolor voluptatem qui in et laborum maiores iure optio sit perferendis adipisci aut quas aliquam ullam sunt debitis voluptatem occaecati.	1f981aae-f40b-4dba-b383-8853d87b23c5	1f981aae-f40b-4dba-b383-8853d87b23c5	2024-10-27 00:14:34.391039	\N	\N	\N	\N	f
1577a842-e358-4ae1-aaa5-f51020d6ddbd	Eaque et illum.	Nostrum quo enim reiciendis laudantium consequatur in earum quis quaerat earum nam illum molestiae totam voluptas facere minima sed et magnam nisi earum ut dolores est laborum qui qui id facere unde facilis odit nisi aliquam facilis quis dolorem voluptate reprehenderit fugit eligendi explicabo dolores reprehenderit aspernatur quod nesciunt rerum.	45370c44-1d4d-4834-8cd5-3551b5d30199	45370c44-1d4d-4834-8cd5-3551b5d30199	2024-10-27 00:14:34.365977	\N	\N	\N	\N	f
164a612d-5213-4dca-983e-4a422233dcbe	Vel odit voluptates.	Dolorem sint accusamus aut et qui adipisci porro id ea dolores voluptas inventore qui molestiae consequatur totam nulla sint laboriosam aut saepe consequatur explicabo consectetur laudantium eum iure velit quo voluptatem corrupti omnis aliquid et repellat itaque dolores accusantium consequatur odio velit amet reprehenderit cupiditate qui nihil aut iure hic.	39ad1877-9e15-4498-88bb-ef6d617a23d2	39ad1877-9e15-4498-88bb-ef6d617a23d2	2024-10-27 00:14:34.370847	\N	\N	\N	\N	f
18bb351d-3819-40ca-821b-7bbb5ab49883	Officia magni perferendis.	Et quisquam illo deserunt aliquid exercitationem excepturi rem non quis ut temporibus porro aperiam laborum repudiandae rem enim facilis quo sed eius non enim atque error reprehenderit in similique in non modi labore provident ut accusamus quia sed voluptatem consectetur velit ut autem nihil accusantium tempora dolorem consequatur quae accusantium.	2fa772f8-0fa4-472b-a154-14cf794d50e2	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-27 00:14:34.366855	\N	\N	\N	\N	f
1e651773-aa16-4035-a02c-971eb4189198	Quis sunt qui.	Voluptatem minus dolor alias aliquid illo deleniti dolor quae illo laboriosam mollitia totam nostrum voluptatem inventore magnam id laborum et sunt fuga voluptatum magni commodi inventore saepe nam commodi inventore odio nulla voluptatem autem vitae accusantium ea qui perferendis debitis voluptates dolor recusandae ducimus iusto optio et ut magni sapiente.	14baebc0-0189-423c-a14c-d62ffe981f63	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-27 00:14:34.376146	\N	\N	\N	\N	f
28513cdd-2a9b-43c0-86ef-ff79ee4abff7	Aut accusamus commodi.	In pariatur non a assumenda molestias odit possimus aut pariatur dolores omnis ad molestiae sunt ullam dolore deserunt asperiores doloremque libero sint architecto aut placeat non soluta sit error eum est maxime molestias itaque facilis doloremque aut error sit quisquam aperiam doloribus rerum voluptatum est minima totam rerum suscipit non.	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.373504	\N	\N	\N	\N	f
2a455855-e05d-44d6-96c6-748114b3cb0e	Voluptatem quia rerum.	Perferendis officiis consequatur reprehenderit dolores sed vero ut eveniet culpa consequatur molestiae fugiat ut aut velit aliquid ut pariatur voluptas commodi aliquid enim ut quia dignissimos ut labore necessitatibus iure quo aut quod nam aspernatur atque molestiae enim perspiciatis culpa et vel et placeat quis ut nulla ut qui autem.	1faf9d72-1396-4e99-935d-547b226327c7	1faf9d72-1396-4e99-935d-547b226327c7	2024-10-27 00:14:34.365549	\N	\N	\N	\N	f
30da2a32-975b-47f3-a347-3a81e4cfd3e4	Debitis temporibus molestiae.	Unde vitae necessitatibus culpa magnam ut dolores ut ut voluptas saepe quis porro error animi impedit itaque sed ea sit mollitia accusantium et est et blanditiis labore tempora molestias ut ducimus similique quis dolorum occaecati perspiciatis ut delectus aperiam vel aut consectetur architecto porro labore eum sed consectetur possimus rerum.	fe1e460d-16ac-4fcd-b512-2413b8cb3256	fe1e460d-16ac-4fcd-b512-2413b8cb3256	2024-10-27 00:14:34.376778	\N	\N	\N	\N	f
407ddd4b-c486-4e07-9276-258ef56f6bc9	Aut repellat aut.	Et excepturi illo quos aut rerum recusandae sint quis dignissimos commodi tempora quod tempore aut dolorem architecto magnam omnis numquam aut iste est voluptatem molestias illo aut atque quia similique sed quo nobis iure totam iusto alias ullam voluptas omnis sapiente quod magnam voluptas porro aspernatur voluptates corrupti ut enim.	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	2024-10-27 00:14:34.375545	\N	\N	\N	\N	f
424afdfe-46e5-461c-8019-f3e5966607a4	Natus ut corrupti.	Temporibus dolor itaque quo sed maiores ut voluptate ipsam molestiae occaecati esse aut qui nulla quia suscipit earum earum odio voluptas reiciendis corporis nesciunt distinctio accusamus aut quis inventore eum dolorem ad soluta cupiditate magni dolores dicta iusto vel similique nemo deleniti quaerat qui suscipit quasi architecto vel quos odio.	35d0da5e-7492-46d3-aaca-17455a353de9	35d0da5e-7492-46d3-aaca-17455a353de9	2024-10-27 00:14:34.369007	\N	\N	\N	\N	f
45323ad6-6ddc-436f-aca9-d4e84de1e7b0	Necessitatibus cupiditate aut.	Odit suscipit voluptas totam dolorum aut quae quisquam saepe recusandae quas ut in facilis reiciendis cumque fuga laudantium autem facilis dignissimos vero voluptates ex in magnam velit ea autem et aut dolorum delectus aut sed maxime id ducimus in ut harum molestiae iusto consequatur harum corporis voluptas adipisci atque libero.	275ddc93-92b8-419a-ab96-baeb34d89c19	275ddc93-92b8-419a-ab96-baeb34d89c19	2024-10-27 00:14:34.380121	\N	\N	\N	\N	f
46355e72-1020-4863-8e28-b06e99861b00	Voluptatibus voluptas eaque.	Autem veritatis ipsam temporibus non eveniet numquam voluptatem libero ut et eveniet provident sit ab illo explicabo rem rerum perferendis vel assumenda vero quo quas eum accusantium quam occaecati ullam consequatur libero repellendus perspiciatis quibusdam corrupti quaerat quia sit est quibusdam quos velit ut modi nulla assumenda doloribus labore nostrum.	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	2024-10-27 00:14:34.390272	\N	\N	\N	\N	f
47117efd-4bb9-4a8e-937d-87e596bef6df	Unde officiis nam.	Et aut eos error corporis possimus quia totam veniam omnis voluptatem quia exercitationem magnam ut minus aliquid ipsa consequuntur atque nostrum fugiat facilis qui hic aut sit dolores aut autem laboriosam est consequatur accusantium in accusamus blanditiis itaque nulla facere et omnis est aperiam voluptates sed ut voluptates dolor voluptas.	8b92673a-ba81-4629-aea9-41444a46a0dc	8b92673a-ba81-4629-aea9-41444a46a0dc	2024-10-27 00:14:34.385911	\N	\N	\N	\N	f
4adce975-4b0b-491e-a984-87c9785e2f62	Perspiciatis qui vitae.	Quibusdam recusandae ut voluptate ullam perspiciatis sit officiis accusamus molestiae maxime ut rem sunt neque velit vel ab optio vel esse vitae culpa natus est nesciunt quidem suscipit repellendus laborum voluptas debitis molestiae non unde est unde labore rerum qui tempore odit dolorem dolores veniam enim ut atque voluptatem tempore.	18e845d8-400b-4d12-a414-9cd440f92677	18e845d8-400b-4d12-a414-9cd440f92677	2024-10-27 00:14:34.378739	\N	\N	\N	\N	f
4afa6810-05d3-4dd0-88e6-776f525ad68b	Esse iusto nostrum.	Voluptate voluptatem error aut illum omnis quo numquam veritatis dicta at voluptas dolorem molestiae provident recusandae consequatur sequi molestiae illum ducimus modi consequatur impedit et velit pariatur repellat libero explicabo eos enim sed deserunt neque temporibus dolor alias quia voluptate ut nostrum nostrum et et et pariatur autem quia tempore.	c6d25490-d32a-410d-be77-5370cc1482a2	c6d25490-d32a-410d-be77-5370cc1482a2	2024-10-27 00:14:34.367289	\N	\N	\N	\N	f
55dafc1a-b3af-4ca1-9b61-c2a32f8ac760	Molestias adipisci rerum.	Omnis occaecati nostrum ex animi dolore aliquid cum incidunt cupiditate commodi earum sed eos dignissimos aspernatur sunt est repellendus neque consequatur error et repellendus corrupti rerum voluptatibus in et aut in autem minus repellat ducimus fugit ea consectetur sunt est doloribus a et qui porro corrupti placeat veniam provident aliquid.	09f405ed-f0c6-422c-847f-0e24f7c74aef	09f405ed-f0c6-422c-847f-0e24f7c74aef	2024-10-27 00:14:34.362155	\N	\N	\N	\N	f
5b32a523-4901-477d-99b0-5c2083a1f503	Velit et voluptatem.	Qui dolorem quia quo dolorem reprehenderit quos similique consequatur voluptas qui similique quam dolores velit ab possimus quisquam molestiae libero sit iure illo enim et aut voluptatem numquam et doloremque dicta quasi modi perspiciatis aspernatur error et eius occaecati assumenda voluptas fugit quasi consequatur enim voluptatum sunt tenetur non mollitia.	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	2024-10-27 00:14:34.360416	\N	\N	\N	\N	f
5c262361-c160-49b8-818f-8044391f0efb	Qui nostrum temporibus.	Natus similique optio libero dolorum quia magni culpa vel sed ut id minus culpa a voluptatem et voluptatem ea magnam repellat perferendis esse quaerat explicabo ut velit accusamus nobis tempora earum et ut sint ipsa dolorum nostrum doloribus quas iure at quod enim eum minus cupiditate unde aut a a.	ae5d22bf-3855-460b-a502-9747f35d6a34	ae5d22bf-3855-460b-a502-9747f35d6a34	2024-10-27 00:14:34.370179	\N	\N	\N	\N	f
6205b8a1-bea3-493c-9793-bce834e1424e	Quia iure delectus.	Qui eligendi quos repellat nihil et aperiam quia architecto tempora sed odio reprehenderit laborum assumenda itaque corporis nihil natus hic magnam reiciendis est iusto ea dolorum explicabo quia minus fugiat nobis ut sit similique facere illum distinctio enim quam id porro illo qui assumenda eum autem cumque animi culpa maiores.	fe1e460d-16ac-4fcd-b512-2413b8cb3256	fe1e460d-16ac-4fcd-b512-2413b8cb3256	2024-10-27 00:14:34.368576	\N	\N	\N	\N	f
63d17d61-1b1a-4dc6-8b70-b993152948b7	Iste dolores commodi.	Sit tenetur est dolor temporibus fugit natus ipsum soluta hic saepe qui pariatur nisi aperiam illum perspiciatis voluptatem quisquam voluptatem consequuntur rem et velit cumque eum eius molestiae impedit in aut aut in molestias dolor quia est voluptatem dolore aut officia autem eaque doloribus molestiae praesentium enim dolor tenetur dolor.	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.384587	\N	\N	\N	\N	f
64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	Sint quos minus.	Pariatur dolorum rem unde est exercitationem recusandae omnis fugiat nisi et illo deleniti quasi dignissimos sint eum eligendi et quia nesciunt autem voluptatum est dolore illo illum quaerat eaque porro quia eligendi molestias aliquam commodi deleniti et aut ipsam est eligendi accusantium occaecati cupiditate quasi delectus culpa commodi quia ut.	5f55d75a-ca3a-4375-bdc4-afb591aef21d	5f55d75a-ca3a-4375-bdc4-afb591aef21d	2024-10-27 00:14:34.361615	\N	\N	\N	\N	f
6949620d-597a-4447-8572-35430780575b	Est accusamus itaque.	Inventore fugiat veritatis suscipit autem molestiae ipsum quia expedita rerum omnis non ut consequuntur dignissimos cum nihil quibusdam voluptas aut architecto sit quod et impedit maiores corporis minima porro rerum illum explicabo dignissimos molestias est nisi soluta iure enim ducimus neque rerum dolorem sunt sit reprehenderit optio aut aut sit.	959b7d55-62bf-42c0-a313-33054551abb5	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-27 00:14:34.391871	\N	\N	\N	\N	f
6a40c092-dbdf-46b3-bf6a-90af37dc2470	Ipsum doloremque odit.	Architecto placeat eius vel possimus qui fugiat adipisci sed dolores repellat non inventore at dicta distinctio libero rem aut est aspernatur rerum fugiat est eum saepe repellat ab debitis officia ipsam qui voluptatum molestiae tempora praesentium tenetur ducimus iusto vero autem beatae et est quibusdam nulla et tempora labore omnis.	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.369471	\N	\N	\N	\N	f
7c3f3594-0276-4e84-98d3-fe51af9231a8	Qui ab repellendus.	Qui aut aut nihil modi quod vel perferendis libero ipsum labore quam tempore sunt dolorem odio magni architecto voluptatem maiores assumenda nemo assumenda culpa omnis et qui dolore sed ut non dicta incidunt et sunt dolor est consectetur occaecati similique et iusto et praesentium aspernatur laborum neque distinctio voluptatem expedita.	e00c9a01-ea24-48db-ac41-4d39c79f9321	e00c9a01-ea24-48db-ac41-4d39c79f9321	2024-10-27 00:14:34.383994	\N	\N	\N	\N	f
7f3769d7-35c7-405a-a6e4-cae832a0d30b	Reiciendis et et.	Dolorem earum expedita non harum omnis saepe in possimus aut architecto quisquam officia aut dolores ex ea ut atque qui est vero amet autem fugiat mollitia voluptatem incidunt sunt repellendus qui accusamus laudantium qui et dicta similique omnis autem reprehenderit quia sapiente ipsum sapiente vero architecto aut aliquid omnis alias.	2fa772f8-0fa4-472b-a154-14cf794d50e2	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-27 00:14:34.382722	\N	\N	\N	\N	f
7f4c4163-e047-4f4f-a704-6d8fa187758b	Nulla harum itaque.	Et perspiciatis officiis aspernatur quo molestias mollitia magni dolorum repudiandae doloremque doloribus sequi natus distinctio quam a dicta velit unde voluptatem maxime aperiam et aut libero rerum aut aliquid dolorem officia veritatis et est omnis accusamus ducimus dolores praesentium blanditiis ipsa sed ducimus ducimus ad iste aperiam accusamus culpa possimus.	22e64c46-97c3-40a7-a4aa-4b11eb838446	22e64c46-97c3-40a7-a4aa-4b11eb838446	2024-10-27 00:14:34.383398	\N	\N	\N	\N	f
8b2e5b51-9894-4dfd-8f19-00158edb8eee	Eaque ipsam et.	Voluptates totam odio omnis commodi distinctio dicta neque ut enim architecto quod doloribus id ut accusantium quidem ipsum voluptas ipsa aliquid ut sit esse exercitationem et est ut voluptatem omnis perferendis soluta fugit ratione temporibus non molestiae voluptatem quod rerum et ut natus eius provident sapiente beatae animi consequatur earum.	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	2024-10-27 00:14:34.381377	\N	\N	\N	\N	f
8d44d803-faa6-495e-a0b3-368d5d3cadb2	Animi non enim.	Consectetur nihil voluptas itaque aut ullam sunt quibusdam sit aut aliquam ipsa illo totam et et cupiditate nulla quis consectetur beatae facilis debitis ea tenetur cupiditate ullam temporibus delectus suscipit excepturi sit cumque explicabo et vero eum assumenda ut est doloribus optio illo illo iure nam distinctio provident rerum praesentium.	6b8b0603-8e07-4181-92ec-ee13f0e768ce	6b8b0603-8e07-4181-92ec-ee13f0e768ce	2024-10-27 00:14:34.378037	\N	\N	\N	\N	f
8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	Aut a itaque.	Consequatur ducimus deleniti alias ut quidem reprehenderit officia officia totam et repellat quia beatae omnis placeat sed blanditiis sint officiis alias dolore commodi enim minima ad impedit error molestiae qui et animi fuga voluptas culpa sint nihil id soluta doloremque reiciendis consectetur dolore est sit neque soluta aut neque sed.	d1372bba-be85-473c-8086-02a7c9890140	d1372bba-be85-473c-8086-02a7c9890140	2024-10-27 00:14:34.356148	\N	\N	\N	\N	f
977d5f30-4fa5-4639-9601-49467c51c384	Sit at ex.	Vel qui delectus ex nam modi perspiciatis quia id odit optio incidunt dolor aut est nesciunt ut expedita odit cumque deserunt asperiores dicta ex eius possimus illum ut est et dolor at sequi saepe harum ea maiores est doloribus facere aut dicta est aut recusandae est id eius sint culpa.	33725381-a183-49ef-b723-e70495ff6066	33725381-a183-49ef-b723-e70495ff6066	2024-10-27 00:14:34.386509	\N	\N	\N	\N	f
9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	Est eum sunt.	Illum voluptatem aliquam aliquam deleniti et aliquid laborum quaerat qui dolores accusamus voluptates ut ut eos consequatur modi omnis natus nam modi aliquam porro natus est qui architecto numquam sed saepe quibusdam reprehenderit modi commodi odit vel similique maxime optio aperiam nihil ex dolor autem voluptatum provident eius doloribus magni.	ed964db3-afac-426e-8988-c2ed54a89510	ed964db3-afac-426e-8988-c2ed54a89510	2024-10-27 00:14:34.366423	\N	\N	\N	\N	f
a2fecc98-afa0-43da-b6fb-cf7f51585c54	Commodi consequatur facilis.	Illo dolorem consequatur optio fugit sed molestiae laboriosam at ea atque quis explicabo nemo et quaerat aliquam adipisci et sint iure molestias odit ea quidem saepe consequatur quam voluptatem quidem inventore totam id est labore eum voluptatum quos eum est eveniet quia et odit temporibus et vel sunt quia aperiam.	f18bc355-4a5c-4012-89a6-0044e4bfe36f	f18bc355-4a5c-4012-89a6-0044e4bfe36f	2024-10-27 00:14:34.364591	\N	\N	\N	\N	f
a3eaf222-b433-4e57-b143-292939df7d38	Quae aut est.	Eius enim similique qui repellat ipsum accusamus maiores ducimus aut laboriosam laborum porro mollitia incidunt eius sed sequi aut quis ratione et eum voluptatem provident ratione impedit vitae at corporis fugiat ab suscipit eum quisquam vitae nihil qui voluptas occaecati laboriosam nostrum quo sint voluptatem mollitia hic suscipit molestiae hic.	07f86036-511f-47d1-b7b7-4543b2eb3303	07f86036-511f-47d1-b7b7-4543b2eb3303	2024-10-27 00:14:34.359892	\N	\N	\N	\N	f
a5c4336f-e13a-4627-8022-4eddc37c3008	Nemo quia ducimus.	Eum quaerat maxime est in nihil nemo sed officiis sequi dolorem praesentium non quia velit nemo numquam quidem iusto nihil occaecati perspiciatis consequatur aut eos quis est perferendis at et possimus ullam illum provident pariatur odit autem ut explicabo at nihil aut et at quo architecto incidunt aliquam illo similique.	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	2024-10-27 00:14:34.360943	\N	\N	\N	\N	f
aac8af68-5ebc-40bf-a970-4593b5cf4dfe	Ea dolores eligendi.	Minus laudantium aliquid hic eum natus quia ut dolor nesciunt ut et dolore exercitationem nobis hic sit distinctio ipsa in ut quae nihil sint nihil sit vel dicta quas ut optio assumenda dolorem sequi omnis autem ut voluptatem nesciunt soluta voluptas illum tempora ut aut doloremque dolores aut omnis odio.	1faf9d72-1396-4e99-935d-547b226327c7	1faf9d72-1396-4e99-935d-547b226327c7	2024-10-27 00:14:34.382027	\N	\N	\N	\N	f
ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	Qui est non.	Laudantium impedit sint error provident minima eum qui nihil dolorem et quas voluptatum repellat soluta est quis omnis et ab quia minus est quia sed vel natus rerum quasi nihil quia quos sed labore sunt suscipit consequatur eligendi quia eos qui voluptatum consequatur suscipit animi itaque quisquam eos inventore exercitationem.	978e2b3f-9c26-41f0-b3c4-cba2e492767f	978e2b3f-9c26-41f0-b3c4-cba2e492767f	2024-10-27 00:14:34.363099	\N	\N	\N	\N	f
b4230aa6-612a-40fe-9920-d919118dc656	Ducimus amet excepturi.	Dolor non qui voluptatem magni magni atque sed nobis dolorem facilis voluptas veritatis voluptas eos debitis eaque distinctio eum tempora minus repellat et vel beatae dolor voluptates et facere fuga quisquam atque ad facilis tempora assumenda itaque maiores quas laboriosam et a ab praesentium magnam repellendus tenetur sint dolorum corrupti.	83c97377-4790-4e12-9b61-c0456fe642b2	83c97377-4790-4e12-9b61-c0456fe642b2	2024-10-27 00:14:34.380771	\N	\N	\N	\N	f
b9d02690-cab8-42e7-89f9-354ae00e25cf	Est laboriosam non.	Ut recusandae provident totam voluptas impedit atque molestiae in et alias quia dolorem eum corrupti nihil aut qui autem et optio similique ipsum quia repudiandae et qui quod esse consequuntur placeat sit voluptatem quidem sunt voluptatem quasi deleniti maxime quos qui qui qui cupiditate vitae deserunt omnis nesciunt eum vel.	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.365083	\N	\N	\N	\N	f
bfc1dd71-fbf0-4b85-8a91-af1686271a2f	Velit cumque ea.	Eos sequi in quidem vel minus qui voluptatem atque eligendi laudantium quia aut voluptate quo temporibus qui quo porro saepe provident aut dolorem dolor odit dolor velit excepturi nam veritatis voluptatem provident voluptatum accusantium sit et eaque libero dolorem sapiente facere quo nemo ipsum sed fuga magnam dolorem tempora officia.	fadd55dc-c457-41a6-9723-c259182f0035	fadd55dc-c457-41a6-9723-c259182f0035	2024-10-27 00:14:34.38526	\N	\N	\N	\N	f
c93495b4-06cd-4c1c-b15d-1c0afd56eb10	Numquam officia consequatur.	Eos eum ab aperiam architecto nostrum autem quia eos voluptas harum suscipit iusto ratione veniam dignissimos blanditiis natus repellat suscipit laborum et aperiam qui quidem rerum fuga eligendi amet libero ad dignissimos expedita id voluptatum officiis nam voluptas eaque quos veritatis at facilis a omnis quasi reiciendis expedita quam ea.	c6d25490-d32a-410d-be77-5370cc1482a2	c6d25490-d32a-410d-be77-5370cc1482a2	2024-10-27 00:14:34.368153	\N	\N	\N	\N	f
c98ac12c-6729-4831-bcb3-473d4cb98a8f	Non cum voluptatum.	Est velit quia libero hic corrupti est doloremque nulla saepe praesentium aut illo aut excepturi et velit omnis consequatur iure ut eos hic dignissimos ea dignissimos excepturi nostrum consequatur et totam voluptate eum est dolores in tenetur consequatur odit magnam atque accusamus distinctio doloribus animi dolores nihil autem voluptate veritatis.	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2024-10-27 00:14:34.3871	\N	\N	\N	\N	f
c9c3aaef-a556-4a26-8ac9-53e4100836a6	Qui et provident.	Sit enim tempore sequi incidunt aut amet nihil laboriosam corporis facere et ea animi consectetur non a perspiciatis odio ipsum iste consectetur quia et maiores aut qui qui aliquam veniam deserunt voluptatibus adipisci provident architecto placeat qui quisquam quia iure ad exercitationem non possimus impedit rerum nesciunt porro porro rerum.	b6663ea1-57ec-4c3a-9597-da421b3c9484	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-27 00:14:34.374937	\N	\N	\N	\N	f
cc594939-2b4b-478c-aca1-18141715add6	Voluptas et consequatur.	Aut excepturi ipsam quis molestiae quod at odit aperiam aut facere sit mollitia ullam cumque quisquam quidem ut velit consequuntur enim architecto illum omnis et beatae est aut occaecati saepe sunt enim amet eum possimus necessitatibus quasi enim neque quia provident voluptas beatae ratione ullam qui ipsa odio recusandae ducimus.	2fa772f8-0fa4-472b-a154-14cf794d50e2	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-27 00:14:34.379405	\N	\N	\N	\N	f
d4d1f3d5-5706-4306-8f06-6065b730b378	Eveniet aut tenetur.	Eveniet nihil facere ut et quam non debitis quos vero nihil dolorem voluptatem ut vel qui culpa possimus rerum necessitatibus laudantium sapiente numquam voluptates ullam quibusdam ipsa harum maxime suscipit aut non ducimus rerum consequatur recusandae sit vero et libero eum nesciunt molestias possimus assumenda id dolorum et dolor sed.	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.362624	\N	\N	\N	\N	f
d5adf278-9369-4d33-a9dc-9985037ce413	Omnis est sint.	Non sapiente voluptatem et consequatur nesciunt sint qui corrupti facilis reiciendis molestias minima aut veniam magnam et nobis culpa doloremque minus culpa ut voluptatem iure ipsum maxime sit illo a dignissimos sunt nihil repellat culpa neque omnis et totam nemo sed repellendus a tenetur officia rerum dolorem et illum et.	eb1b0535-b7f3-430e-b91c-c1feea653f5f	eb1b0535-b7f3-430e-b91c-c1feea653f5f	2024-10-27 00:14:34.367726	\N	\N	\N	\N	f
f2bceb45-a945-4eef-80b5-e93128060efb	Distinctio commodi nemo.	Minima ea tempora labore corrupti consequatur odio nostrum eveniet ipsa voluptates id aut eum nisi amet sed animi saepe non reiciendis ea fugiat non necessitatibus explicabo iste ut officiis autem autem omnis quia perferendis rerum est eum eaque labore inventore eos et sint omnis alias sint eius explicabo nulla delectus.	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2024-10-27 00:14:34.3774	\N	\N	\N	\N	f
fa08b1d2-6fa6-4b96-9738-57d8b2926053	Incidunt architecto iure.	Sint maxime temporibus sunt eum qui aut ut quae excepturi laudantium ea fuga sit ad quia autem ut mollitia soluta consequatur voluptas sequi fugit tempora in rerum eum quod rerum reiciendis rem rerum aliquid dolorum molestiae minus saepe alias eligendi fuga autem eos voluptas praesentium mollitia voluptas assumenda consequatur error.	612e214e-4fe6-4b17-b9af-8b8b26bf180e	612e214e-4fe6-4b17-b9af-8b8b26bf180e	2024-10-27 00:14:34.392702	\N	\N	\N	\N	f
\.


--
-- TOC entry 2974 (class 0 OID 16899)
-- Dependencies: 204
-- Data for Name: groups_members; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.groups_members (id, role, group_id, user_profile_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
005abfa4-401d-41a7-b955-2f7013079b0f	2	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	b6d54f8d-b08c-4f88-9db9-00008875256f	b6d54f8d-b08c-4f88-9db9-00008875256f	2024-10-27 00:14:34.840056	\N	\N	\N	\N	f
00d2745a-364d-4ceb-b641-7a3d28995af1	0	f2bceb45-a945-4eef-80b5-e93128060efb	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.759119	\N	\N	\N	\N	f
01a1df97-81d2-4926-83ee-9f51a95c9eab	2	b9d02690-cab8-42e7-89f9-354ae00e25cf	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2024-10-27 00:14:34.856986	\N	\N	\N	\N	f
01c2414c-4f5f-40b1-a34b-ec0deb63f931	0	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	8f722abd-0123-4494-b71c-a21943484a3c	8f722abd-0123-4494-b71c-a21943484a3c	2024-10-27 00:14:34.829338	\N	\N	\N	\N	f
027be3ba-3ded-44d2-8dcb-a3e422619ee1	0	8b2e5b51-9894-4dfd-8f19-00158edb8eee	b3243d6a-7be2-4c83-8a89-dfd4a1976095	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2024-10-27 00:14:34.806396	\N	\N	\N	\N	f
02b0e8f9-34b5-4e97-aaf1-00d6d20bb617	2	7f3769d7-35c7-405a-a6e4-cae832a0d30b	2e6b7127-5e54-43eb-a21f-64c57143824d	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-27 00:14:34.824714	\N	\N	\N	\N	f
02bd2d9b-9439-4609-a0a7-7b9f283f23d5	2	5c262361-c160-49b8-818f-8044391f0efb	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.7089	\N	\N	\N	\N	f
038b36b8-91ac-4f3d-b3a1-78c764ee019f	1	c98ac12c-6729-4831-bcb3-473d4cb98a8f	9f64a38d-8cdd-4a21-a529-9747a9331998	9f64a38d-8cdd-4a21-a529-9747a9331998	2024-10-27 00:14:34.889733	\N	\N	\N	\N	f
05260073-dc22-4ec7-9072-e5202c7a6416	0	b9d02690-cab8-42e7-89f9-354ae00e25cf	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.778878	\N	\N	\N	\N	f
062f4bf4-62da-4292-a2e7-3345182e20b0	0	47117efd-4bb9-4a8e-937d-87e596bef6df	e095bbae-68d2-4077-9036-697c526736d4	e095bbae-68d2-4077-9036-697c526736d4	2024-10-27 00:14:34.732755	\N	\N	\N	\N	f
06a1efed-6516-4d7a-ac55-27096228f2eb	0	cc594939-2b4b-478c-aca1-18141715add6	d1372bba-be85-473c-8086-02a7c9890140	d1372bba-be85-473c-8086-02a7c9890140	2024-10-27 00:14:34.746264	\N	\N	\N	\N	f
06afc684-621e-47b2-a680-80ffbf173fe1	2	a5c4336f-e13a-4627-8022-4eddc37c3008	eba19f8f-0936-45eb-88bc-9c83772a1d93	eba19f8f-0936-45eb-88bc-9c83772a1d93	2024-10-27 00:14:34.728706	\N	\N	\N	\N	f
06fc5848-0060-408b-b5e5-83880aeef7f3	0	7c3f3594-0276-4e84-98d3-fe51af9231a8	13ba9424-00b3-40a6-92c8-a9426207a2d9	13ba9424-00b3-40a6-92c8-a9426207a2d9	2024-10-27 00:14:34.751085	\N	\N	\N	\N	f
076d29bd-65a9-4549-b034-06b6be264862	0	977d5f30-4fa5-4639-9601-49467c51c384	78532cb2-f350-4c98-bce2-e94afd8369c6	78532cb2-f350-4c98-bce2-e94afd8369c6	2024-10-27 00:14:34.815871	\N	\N	\N	\N	f
088838ed-fe79-44f4-980c-8c26a176f5a0	2	18bb351d-3819-40ca-821b-7bbb5ab49883	3d8be820-f83f-4579-b8e2-a8c4b5465d69	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-27 00:14:34.836363	\N	\N	\N	\N	f
09c9f2ec-9b26-439a-9ebe-89caf3585110	0	b9d02690-cab8-42e7-89f9-354ae00e25cf	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2024-10-27 00:14:34.815125	\N	\N	\N	\N	f
09d91c80-9207-448c-878b-31ed06445b37	1	0f087e27-7a21-45ea-b6d3-7296c5360cc3	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.729319	\N	\N	\N	\N	f
09de7b16-278d-4acf-b3f8-8ecd8fc9c907	0	7f4c4163-e047-4f4f-a704-6d8fa187758b	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	2024-10-27 00:14:34.71075	\N	\N	\N	\N	f
0a7d1161-b9b2-4f63-a489-85ae5d1e96c2	1	46355e72-1020-4863-8e28-b06e99861b00	b55f5bbd-4b95-448a-b38b-a1429002854b	b55f5bbd-4b95-448a-b38b-a1429002854b	2024-10-27 00:14:34.726875	\N	\N	\N	\N	f
0ac2199a-90a1-4c99-bced-8ab098c488dc	2	47117efd-4bb9-4a8e-937d-87e596bef6df	384d21de-6a0f-4c92-b0ef-540ff97079bc	384d21de-6a0f-4c92-b0ef-540ff97079bc	2024-10-27 00:14:34.848857	\N	\N	\N	\N	f
0afcbdd9-1ed5-402a-93b3-3685a1f17705	2	fa08b1d2-6fa6-4b96-9738-57d8b2926053	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-27 00:14:34.855753	\N	\N	\N	\N	f
0b8b6757-0421-48a8-8c36-1a6312cc9be6	2	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	978e2b3f-9c26-41f0-b3c4-cba2e492767f	978e2b3f-9c26-41f0-b3c4-cba2e492767f	2024-10-27 00:14:34.724737	\N	\N	\N	\N	f
0bae4913-42bc-4dae-b04d-4bf2b9d8b8a0	1	18bb351d-3819-40ca-821b-7bbb5ab49883	7b42cb26-668a-4037-8ffc-68058704a460	7b42cb26-668a-4037-8ffc-68058704a460	2024-10-27 00:14:34.79408	\N	\N	\N	\N	f
0d642b21-25ea-43e1-8f53-da2c0e4bf707	1	424afdfe-46e5-461c-8019-f3e5966607a4	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	2024-10-27 00:14:34.731199	\N	\N	\N	\N	f
0da79f3a-25fa-45c9-bc81-949c5872db49	0	f2bceb45-a945-4eef-80b5-e93128060efb	45370c44-1d4d-4834-8cd5-3551b5d30199	45370c44-1d4d-4834-8cd5-3551b5d30199	2024-10-27 00:14:34.856355	\N	\N	\N	\N	f
0e39a7bb-8b70-4dcc-a049-acb7545b826d	2	06314656-9772-4193-b105-edd0c136d72f	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	2024-10-27 00:14:34.76266	\N	\N	\N	\N	f
0e4a6160-803f-430e-bd2c-672a1edfff5d	0	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	9f64a38d-8cdd-4a21-a529-9747a9331998	9f64a38d-8cdd-4a21-a529-9747a9331998	2024-10-27 00:14:34.872696	\N	\N	\N	\N	f
0ecdb351-5e1f-472b-9e00-03dd34325875	2	30da2a32-975b-47f3-a347-3a81e4cfd3e4	5f55d75a-ca3a-4375-bdc4-afb591aef21d	5f55d75a-ca3a-4375-bdc4-afb591aef21d	2024-10-27 00:14:34.835584	\N	\N	\N	\N	f
10d22614-585c-4f50-9538-65ae1f7b7c4e	2	b4230aa6-612a-40fe-9920-d919118dc656	fe1e460d-16ac-4fcd-b512-2413b8cb3256	fe1e460d-16ac-4fcd-b512-2413b8cb3256	2024-10-27 00:14:34.743223	\N	\N	\N	\N	f
11259205-3896-4455-b812-b6fbbe515892	0	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	3d8be820-f83f-4579-b8e2-a8c4b5465d69	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-27 00:14:34.896286	\N	\N	\N	\N	f
12a25955-1951-41e1-a221-fc5e160c5313	0	424afdfe-46e5-461c-8019-f3e5966607a4	33725381-a183-49ef-b723-e70495ff6066	33725381-a183-49ef-b723-e70495ff6066	2024-10-27 00:14:34.85117	\N	\N	\N	\N	f
138c9a27-e530-417c-bd5b-f5365321f24c	1	977d5f30-4fa5-4639-9601-49467c51c384	eba19f8f-0936-45eb-88bc-9c83772a1d93	eba19f8f-0936-45eb-88bc-9c83772a1d93	2024-10-27 00:14:34.744415	\N	\N	\N	\N	f
159f0681-c2aa-4c31-b6a8-73f152f3f7ab	2	63d17d61-1b1a-4dc6-8b70-b993152948b7	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.78243	\N	\N	\N	\N	f
168ae1aa-9812-4b98-a5d9-d66f1ecd49f0	0	1e651773-aa16-4035-a02c-971eb4189198	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.825339	\N	\N	\N	\N	f
1777db57-c562-42cc-ac63-5e49f95b0457	1	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2024-10-27 00:14:34.751813	\N	\N	\N	\N	f
17a854f6-05fb-4602-97ab-33d7fc5153f3	1	4adce975-4b0b-491e-a984-87c9785e2f62	74d9ea46-5729-454f-b994-8faee093ddef	74d9ea46-5729-454f-b994-8faee093ddef	2024-10-27 00:14:34.783001	\N	\N	\N	\N	f
1a92a2bb-1cbe-4d6f-beee-3ccb94c56432	1	f2bceb45-a945-4eef-80b5-e93128060efb	39ad1877-9e15-4498-88bb-ef6d617a23d2	39ad1877-9e15-4498-88bb-ef6d617a23d2	2024-10-27 00:14:34.869372	\N	\N	\N	\N	f
1b65c38d-135b-4923-ba67-23ebefa42978	0	f2bceb45-a945-4eef-80b5-e93128060efb	b6663ea1-57ec-4c3a-9597-da421b3c9484	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-27 00:14:34.788586	\N	\N	\N	\N	f
1bce6e96-2c30-4f91-9e01-02d81c6990f6	0	c9c3aaef-a556-4a26-8ac9-53e4100836a6	1f981aae-f40b-4dba-b383-8853d87b23c5	1f981aae-f40b-4dba-b383-8853d87b23c5	2024-10-27 00:14:34.773567	\N	\N	\N	\N	f
1beec72d-c9c1-46e9-a83b-68e8bfd1e1c9	1	5c262361-c160-49b8-818f-8044391f0efb	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	2024-10-27 00:14:34.724034	\N	\N	\N	\N	f
1cbe2e66-922c-409f-9d83-ac180434a9a3	0	b9d02690-cab8-42e7-89f9-354ae00e25cf	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	2024-10-27 00:14:34.807625	\N	\N	\N	\N	f
1d621ba7-4ab4-4198-a79f-d3cbb3705fe8	0	8d44d803-faa6-495e-a0b3-368d5d3cadb2	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	2024-10-27 00:14:34.896863	\N	\N	\N	\N	f
1db243b1-4fd2-4ae5-abf6-9c411894e00f	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	2024-10-27 00:14:34.748112	\N	\N	\N	\N	f
2002d357-fd40-4d00-b95c-042aa331da91	2	f2bceb45-a945-4eef-80b5-e93128060efb	3652e96a-9dc0-4f12-817c-1ca7f05807e6	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2024-10-27 00:14:34.755593	\N	\N	\N	\N	f
200a4553-a9bb-43b8-b7f2-ca30398afc2a	1	28513cdd-2a9b-43c0-86ef-ff79ee4abff7	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	2024-10-27 00:14:34.86464	\N	\N	\N	\N	f
231179b9-9f91-46e8-8ac2-b92d32bb388f	0	7f3769d7-35c7-405a-a6e4-cae832a0d30b	fadd55dc-c457-41a6-9723-c259182f0035	fadd55dc-c457-41a6-9723-c259182f0035	2024-10-27 00:14:34.865946	\N	\N	\N	\N	f
23bc1ed3-560b-4977-920a-4036e31540b9	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	18e845d8-400b-4d12-a414-9cd440f92677	18e845d8-400b-4d12-a414-9cd440f92677	2024-10-27 00:14:34.792228	\N	\N	\N	\N	f
24442dd1-2db8-4be5-a929-566e24a4d6ea	2	7f3769d7-35c7-405a-a6e4-cae832a0d30b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.881408	\N	\N	\N	\N	f
247457a8-eede-486d-8477-f302df075116	1	02d11521-1d02-4af8-b31a-74fa8bb08778	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.863906	\N	\N	\N	\N	f
24d75616-031f-4268-b964-81dc7352f4af	1	6205b8a1-bea3-493c-9793-bce834e1424e	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	2024-10-27 00:14:34.772951	\N	\N	\N	\N	f
25f80c72-c08f-45e1-998c-6423240a4b39	1	6949620d-597a-4447-8572-35430780575b	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.766074	\N	\N	\N	\N	f
273beed0-c0c7-4b91-84d5-e63bbebdf470	1	6949620d-597a-4447-8572-35430780575b	b0d1d45b-c71b-4578-8ac0-01c30b49131b	b0d1d45b-c71b-4578-8ac0-01c30b49131b	2024-10-27 00:14:34.740868	\N	\N	\N	\N	f
279df28b-02d2-4fc1-b106-9917f9559e0c	0	f2bceb45-a945-4eef-80b5-e93128060efb	1cc85c40-c092-4bee-adeb-3dc17e304563	1cc85c40-c092-4bee-adeb-3dc17e304563	2024-10-27 00:14:34.879555	\N	\N	\N	\N	f
28c96991-f376-4249-a9a6-6f362fc42322	1	164a612d-5213-4dca-983e-4a422233dcbe	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	2024-10-27 00:14:34.838252	\N	\N	\N	\N	f
2bf1c689-58fe-444e-ba1a-a0a4827cec74	2	4afa6810-05d3-4dd0-88e6-776f525ad68b	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.823543	\N	\N	\N	\N	f
30b0786a-82a5-4237-a008-4dc3fad3bfc6	1	8d44d803-faa6-495e-a0b3-368d5d3cadb2	13ba9424-00b3-40a6-92c8-a9426207a2d9	13ba9424-00b3-40a6-92c8-a9426207a2d9	2024-10-27 00:14:34.766784	\N	\N	\N	\N	f
310484f2-568b-48a2-b737-5c2575d9760e	2	6a40c092-dbdf-46b3-bf6a-90af37dc2470	6700632c-6c3b-4d7e-81dd-8b2151b60502	6700632c-6c3b-4d7e-81dd-8b2151b60502	2024-10-27 00:14:34.734909	\N	\N	\N	\N	f
315ee80a-0d72-4732-90a3-e6960f55dfbd	1	30da2a32-975b-47f3-a347-3a81e4cfd3e4	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.71576	\N	\N	\N	\N	f
322e9428-bc9f-4ea5-8900-ddaa04e75aa6	2	8d44d803-faa6-495e-a0b3-368d5d3cadb2	d1372bba-be85-473c-8086-02a7c9890140	d1372bba-be85-473c-8086-02a7c9890140	2024-10-27 00:14:34.883967	\N	\N	\N	\N	f
33354871-3afb-4246-9d19-4074d05f7f62	2	6a40c092-dbdf-46b3-bf6a-90af37dc2470	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.848207	\N	\N	\N	\N	f
33da809a-03db-4273-a91b-be22a778fced	1	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	b6663ea1-57ec-4c3a-9597-da421b3c9484	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-27 00:14:34.805796	\N	\N	\N	\N	f
34084947-89cf-42d8-b909-d4852e55e5f1	1	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	2fa772f8-0fa4-472b-a154-14cf794d50e2	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-27 00:14:34.822933	\N	\N	\N	\N	f
34777e02-ae00-441d-bb7f-29aba7bfe3a6	2	c9c3aaef-a556-4a26-8ac9-53e4100836a6	eb1b0535-b7f3-430e-b91c-c1feea653f5f	eb1b0535-b7f3-430e-b91c-c1feea653f5f	2024-10-27 00:14:34.897548	\N	\N	\N	\N	f
34a28ca6-17bb-451e-8188-c5d0b39304d4	0	977d5f30-4fa5-4639-9601-49467c51c384	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.756232	\N	\N	\N	\N	f
351cdef6-6f99-4ef2-96b9-78cda41b91d2	0	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	20105f5a-82e0-4763-b12c-7a5ddc9abf83	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-27 00:14:34.799936	\N	\N	\N	\N	f
35e2fb2c-17ec-4965-b655-71f2a7235187	2	1080d248-b0ca-4124-accd-2ba2f0d30cba	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	2024-10-27 00:14:34.819703	\N	\N	\N	\N	f
35fc736d-dfa0-4b56-8eba-b308ff5281fc	1	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	20105f5a-82e0-4763-b12c-7a5ddc9abf83	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-27 00:14:34.78584	\N	\N	\N	\N	f
36cec5f6-70a8-49a3-92f1-799090e37ac4	1	1e651773-aa16-4035-a02c-971eb4189198	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2024-10-27 00:14:34.743817	\N	\N	\N	\N	f
36f502ff-52c3-4f15-aed2-01dc8813f85d	2	8b2e5b51-9894-4dfd-8f19-00158edb8eee	612e214e-4fe6-4b17-b9af-8b8b26bf180e	612e214e-4fe6-4b17-b9af-8b8b26bf180e	2024-10-27 00:14:34.824118	\N	\N	\N	\N	f
3710777b-6007-4a1e-80b2-5e07867055e2	2	02d11521-1d02-4af8-b31a-74fa8bb08778	384d21de-6a0f-4c92-b0ef-540ff97079bc	384d21de-6a0f-4c92-b0ef-540ff97079bc	2024-10-27 00:14:34.71456	\N	\N	\N	\N	f
38ba456d-61ff-411a-b381-aa6388f906a7	1	d5adf278-9369-4d33-a9dc-9985037ce413	134e6153-f93b-4592-8bd7-ae30e9321017	134e6153-f93b-4592-8bd7-ae30e9321017	2024-10-27 00:14:34.889143	\N	\N	\N	\N	f
3ba82264-4e0c-4ce1-8259-36ea2d6e0874	1	f2bceb45-a945-4eef-80b5-e93128060efb	53453386-8816-485f-9a08-22c07cf29d22	53453386-8816-485f-9a08-22c07cf29d22	2024-10-27 00:14:34.799326	\N	\N	\N	\N	f
3c174924-4159-4d6e-b445-270ace9491d4	1	8d44d803-faa6-495e-a0b3-368d5d3cadb2	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.752525	\N	\N	\N	\N	f
3c998057-7629-4554-aa8e-479104650972	0	1577a842-e358-4ae1-aaa5-f51020d6ddbd	be26aee1-0512-4e6a-8313-5c18759958a9	be26aee1-0512-4e6a-8313-5c18759958a9	2024-10-27 00:14:34.747477	\N	\N	\N	\N	f
3d5e1cb0-d1ac-4541-bde8-0d7ee61bf67b	0	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.803941	\N	\N	\N	\N	f
3f97b4e4-274e-4cdd-b16a-0865412d37e8	1	aac8af68-5ebc-40bf-a970-4593b5cf4dfe	1faf9d72-1396-4e99-935d-547b226327c7	1faf9d72-1396-4e99-935d-547b226327c7	2024-10-27 00:14:34.88021	\N	\N	\N	\N	f
3fd036c9-8083-49c0-afd5-309740f19bc9	1	06314656-9772-4193-b105-edd0c136d72f	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-27 00:14:34.748742	\N	\N	\N	\N	f
3fdffe66-7ed4-454b-9477-e378d9d46bc8	2	6a40c092-dbdf-46b3-bf6a-90af37dc2470	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.805199	\N	\N	\N	\N	f
40b0b283-bdcd-474f-a08c-058ef2401d61	0	b9d02690-cab8-42e7-89f9-354ae00e25cf	143437a3-503e-4e95-911d-4c6571ddea8e	143437a3-503e-4e95-911d-4c6571ddea8e	2024-10-27 00:14:34.845586	\N	\N	\N	\N	f
40c972d8-23fb-4c34-8692-fbf733056f78	0	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	2024-10-27 00:14:34.875456	\N	\N	\N	\N	f
43656bb5-5df1-4be1-b2bc-f2250e04fdad	2	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	7374bc88-8afb-4236-9fa0-d75adad253a0	7374bc88-8afb-4236-9fa0-d75adad253a0	2024-10-27 00:14:34.775358	\N	\N	\N	\N	f
43dd5a7d-14e8-43d0-8a0b-3a2bcd84fd96	0	d5adf278-9369-4d33-a9dc-9985037ce413	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.853917	\N	\N	\N	\N	f
443691e9-4b3d-420c-858a-5b958245370d	2	a5c4336f-e13a-4627-8022-4eddc37c3008	439c9800-35c9-48ee-8549-9c293a107743	439c9800-35c9-48ee-8549-9c293a107743	2024-10-27 00:14:34.787734	\N	\N	\N	\N	f
4470db4a-77bb-4abb-bca6-3149fb2d8f6a	1	b9d02690-cab8-42e7-89f9-354ae00e25cf	3652e96a-9dc0-4f12-817c-1ca7f05807e6	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2024-10-27 00:14:34.76989	\N	\N	\N	\N	f
44ea2ceb-e7fe-4778-b693-17598e2a46ad	2	a3eaf222-b433-4e57-b143-292939df7d38	bb05cc9c-87a1-4d43-b253-d172e2117ff2	bb05cc9c-87a1-4d43-b253-d172e2117ff2	2024-10-27 00:14:34.745627	\N	\N	\N	\N	f
45dfdb87-a718-4b66-9ce2-9c57016b4e9b	1	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	13ba9424-00b3-40a6-92c8-a9426207a2d9	13ba9424-00b3-40a6-92c8-a9426207a2d9	2024-10-27 00:14:34.846798	\N	\N	\N	\N	f
4805bae4-239f-443f-85eb-b59d9f5e782a	0	47117efd-4bb9-4a8e-937d-87e596bef6df	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.809583	\N	\N	\N	\N	f
4a3c088f-b60a-4524-a926-89cb3a1dff1e	1	28513cdd-2a9b-43c0-86ef-ff79ee4abff7	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-27 00:14:34.820312	\N	\N	\N	\N	f
4a5ad9e8-53c5-431c-b127-adc092769b00	0	4afa6810-05d3-4dd0-88e6-776f525ad68b	33725381-a183-49ef-b723-e70495ff6066	33725381-a183-49ef-b723-e70495ff6066	2024-10-27 00:14:34.833847	\N	\N	\N	\N	f
4a5c52ec-dea0-4441-b0fa-14f125c3df35	1	30da2a32-975b-47f3-a347-3a81e4cfd3e4	84609dec-b050-496e-81be-301a1334919a	84609dec-b050-496e-81be-301a1334919a	2024-10-27 00:14:34.808263	\N	\N	\N	\N	f
4c733663-31db-4c2b-8ac2-113481d36446	1	55dafc1a-b3af-4ca1-9b61-c2a32f8ac760	1cc85c40-c092-4bee-adeb-3dc17e304563	1cc85c40-c092-4bee-adeb-3dc17e304563	2024-10-27 00:14:34.791034	\N	\N	\N	\N	f
4c9b5224-773b-4a55-b34f-37bf0e4b6f60	2	7f3769d7-35c7-405a-a6e4-cae832a0d30b	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	2024-10-27 00:14:34.861976	\N	\N	\N	\N	f
4d9b60dd-8e9e-4876-be01-2f2f507988e5	0	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	22e64c46-97c3-40a7-a4aa-4b11eb838446	22e64c46-97c3-40a7-a4aa-4b11eb838446	2024-10-27 00:14:34.888574	\N	\N	\N	\N	f
4f22c3fe-d409-4d61-b57b-ccbde80073ce	2	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	5f55d75a-ca3a-4375-bdc4-afb591aef21d	5f55d75a-ca3a-4375-bdc4-afb591aef21d	2024-10-27 00:14:34.716463	\N	\N	\N	\N	f
4f4f8606-3cdb-4170-8c72-4afb604374ad	2	d4d1f3d5-5706-4306-8f06-6065b730b378	ae5d22bf-3855-460b-a502-9747f35d6a34	ae5d22bf-3855-460b-a502-9747f35d6a34	2024-10-27 00:14:34.753877	\N	\N	\N	\N	f
4f6c0585-1ab1-4a21-9d30-9bb10973c779	0	6205b8a1-bea3-493c-9793-bce834e1424e	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.791626	\N	\N	\N	\N	f
51eee73e-a019-4489-bd4b-fa5a3d1bc1af	1	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	7b42cb26-668a-4037-8ffc-68058704a460	7b42cb26-668a-4037-8ffc-68058704a460	2024-10-27 00:14:34.850273	\N	\N	\N	\N	f
53382258-2f46-4875-874c-0f58bb990e4d	0	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	b1469423-4113-490e-bcd6-b79a146c3f81	b1469423-4113-490e-bcd6-b79a146c3f81	2024-10-27 00:14:34.722757	\N	\N	\N	\N	f
535d2d1a-4415-4aa6-93dc-3ffb54b0cb08	2	4afa6810-05d3-4dd0-88e6-776f525ad68b	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	2024-10-27 00:14:34.886775	\N	\N	\N	\N	f
53e7bf97-9225-47db-b0e6-a7a828986392	1	30da2a32-975b-47f3-a347-3a81e4cfd3e4	fa846317-fe54-4f52-b8d6-6a618387a5da	fa846317-fe54-4f52-b8d6-6a618387a5da	2024-10-27 00:14:34.852744	\N	\N	\N	\N	f
548fd36f-2aec-4b8e-ba01-7923407663d8	1	45323ad6-6ddc-436f-aca9-d4e84de1e7b0	b3243d6a-7be2-4c83-8a89-dfd4a1976095	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2024-10-27 00:14:34.870647	\N	\N	\N	\N	f
54f8d5a5-db23-44c2-92f0-21016930924a	1	02d11521-1d02-4af8-b31a-74fa8bb08778	13ba9424-00b3-40a6-92c8-a9426207a2d9	13ba9424-00b3-40a6-92c8-a9426207a2d9	2024-10-27 00:14:34.810188	\N	\N	\N	\N	f
55b6d2dd-a7ec-4431-9298-ef393cc9bfe3	2	424afdfe-46e5-461c-8019-f3e5966607a4	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	2024-10-27 00:14:34.804553	\N	\N	\N	\N	f
561765ea-25a2-4ea7-9456-9c97085f71d4	0	7c3f3594-0276-4e84-98d3-fe51af9231a8	be26aee1-0512-4e6a-8313-5c18759958a9	be26aee1-0512-4e6a-8313-5c18759958a9	2024-10-27 00:14:34.844296	\N	\N	\N	\N	f
56742139-9dd2-4a3c-a115-5a0add0e974f	2	18bb351d-3819-40ca-821b-7bbb5ab49883	74d9ea46-5729-454f-b994-8faee093ddef	74d9ea46-5729-454f-b994-8faee093ddef	2024-10-27 00:14:34.801228	\N	\N	\N	\N	f
5811c253-c1c9-4c63-a97e-183b06c3bec7	1	f2bceb45-a945-4eef-80b5-e93128060efb	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.865295	\N	\N	\N	\N	f
589f9306-12b6-4246-8ae1-85e53e3542c2	2	8d44d803-faa6-495e-a0b3-368d5d3cadb2	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-27 00:14:34.720906	\N	\N	\N	\N	f
5c759f97-faaa-4424-b67f-96e25d3b4e68	0	4afa6810-05d3-4dd0-88e6-776f525ad68b	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	2024-10-27 00:14:34.781143	\N	\N	\N	\N	f
5cb8c1d0-fac2-4e94-a237-e2e7a61a1de2	1	1e651773-aa16-4035-a02c-971eb4189198	eba19f8f-0936-45eb-88bc-9c83772a1d93	eba19f8f-0936-45eb-88bc-9c83772a1d93	2024-10-27 00:14:34.887376	\N	\N	\N	\N	f
5ceeacfa-1bd3-4f1b-924f-82219044b965	2	a5c4336f-e13a-4627-8022-4eddc37c3008	fadd55dc-c457-41a6-9723-c259182f0035	fadd55dc-c457-41a6-9723-c259182f0035	2024-10-27 00:14:34.872041	\N	\N	\N	\N	f
5fd9ccf9-e92e-4fea-8432-38f626bcdd89	1	8b2e5b51-9894-4dfd-8f19-00158edb8eee	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.833187	\N	\N	\N	\N	f
5ffa6906-fd71-4ffe-a829-c4585413675f	0	a2fecc98-afa0-43da-b6fb-cf7f51585c54	2e6b7127-5e54-43eb-a21f-64c57143824d	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-27 00:14:34.784136	\N	\N	\N	\N	f
608a77e5-b582-4212-bc4c-12629849d18a	0	4adce975-4b0b-491e-a984-87c9785e2f62	f015b253-2d06-44b2-8f52-1ae49c1a241c	f015b253-2d06-44b2-8f52-1ae49c1a241c	2024-10-27 00:14:34.739438	\N	\N	\N	\N	f
61b453f4-36cc-404b-b598-8fdd19184478	1	4adce975-4b0b-491e-a984-87c9785e2f62	ae5d22bf-3855-460b-a502-9747f35d6a34	ae5d22bf-3855-460b-a502-9747f35d6a34	2024-10-27 00:14:34.893824	\N	\N	\N	\N	f
61efc65b-349a-410f-ba5f-32ce822f9bc4	2	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	2024-10-27 00:14:34.819095	\N	\N	\N	\N	f
6209881e-9c9d-4f34-9687-42a74bee2f4e	1	6949620d-597a-4447-8572-35430780575b	1faf9d72-1396-4e99-935d-547b226327c7	1faf9d72-1396-4e99-935d-547b226327c7	2024-10-27 00:14:34.851836	\N	\N	\N	\N	f
63dccc38-37c2-452e-a399-1e12cd50cba7	1	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	e21d9b47-d1bb-4c02-9704-acff338cf963	e21d9b47-d1bb-4c02-9704-acff338cf963	2024-10-27 00:14:34.754446	\N	\N	\N	\N	f
640bee24-f4fe-45f7-b11a-4d153653537c	1	5b32a523-4901-477d-99b0-5c2083a1f503	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	2024-10-27 00:14:34.740254	\N	\N	\N	\N	f
65ef0af3-9320-4e4b-9989-a4cd39c5664f	0	1080d248-b0ca-4124-accd-2ba2f0d30cba	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	2024-10-27 00:14:34.792869	\N	\N	\N	\N	f
65f05ba3-fe3c-4b0b-b474-9eb9ecab9e9e	2	a3eaf222-b433-4e57-b143-292939df7d38	959b7d55-62bf-42c0-a313-33054551abb5	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-27 00:14:34.816536	\N	\N	\N	\N	f
673c24e1-45b4-4682-8f56-a1c7bc3e5766	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	6e132241-d674-4195-b8c5-b6b4d320e3ce	6e132241-d674-4195-b8c5-b6b4d320e3ce	2024-10-27 00:14:34.863191	\N	\N	\N	\N	f
67ae62d3-3ebf-4655-b060-4e45c70c56f1	0	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.762085	\N	\N	\N	\N	f
68275240-d233-41b2-b3b8-a817fe4d7169	0	6205b8a1-bea3-493c-9793-bce834e1424e	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.713969	\N	\N	\N	\N	f
682d082d-4142-458b-9280-8de35b5bf3c4	2	407ddd4b-c486-4e07-9276-258ef56f6bc9	eba19f8f-0936-45eb-88bc-9c83772a1d93	eba19f8f-0936-45eb-88bc-9c83772a1d93	2024-10-27 00:14:34.70828	\N	\N	\N	\N	f
69196523-bd9b-4128-b37b-a92000c74849	1	63d17d61-1b1a-4dc6-8b70-b993152948b7	1cc85c40-c092-4bee-adeb-3dc17e304563	1cc85c40-c092-4bee-adeb-3dc17e304563	2024-10-27 00:14:34.729987	\N	\N	\N	\N	f
695a27aa-bff7-456e-b274-55bfca7ba753	0	02d11521-1d02-4af8-b31a-74fa8bb08778	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	2024-10-27 00:14:34.797335	\N	\N	\N	\N	f
69fd1eea-f402-4041-a896-97d46dd02e46	1	cc594939-2b4b-478c-aca1-18141715add6	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.831962	\N	\N	\N	\N	f
6bd39db1-d2b3-4880-b6cc-308cd5ffe896	2	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	22e64c46-97c3-40a7-a4aa-4b11eb838446	22e64c46-97c3-40a7-a4aa-4b11eb838446	2024-10-27 00:14:34.835016	\N	\N	\N	\N	f
6cb8ca71-854f-402c-b55c-87e9b33660da	0	47117efd-4bb9-4a8e-937d-87e596bef6df	d1372bba-be85-473c-8086-02a7c9890140	d1372bba-be85-473c-8086-02a7c9890140	2024-10-27 00:14:34.810792	\N	\N	\N	\N	f
6dcf5ad6-6b28-43c9-bbff-37bf60f1237e	1	1577a842-e358-4ae1-aaa5-f51020d6ddbd	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.895066	\N	\N	\N	\N	f
6ec3be86-b93a-419d-b626-d604accf4054	1	1577a842-e358-4ae1-aaa5-f51020d6ddbd	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.807025	\N	\N	\N	\N	f
702b2249-280c-432d-9e3c-cbb5952167a4	2	5b32a523-4901-477d-99b0-5c2083a1f503	22e64c46-97c3-40a7-a4aa-4b11eb838446	22e64c46-97c3-40a7-a4aa-4b11eb838446	2024-10-27 00:14:34.735555	\N	\N	\N	\N	f
7084ee9e-d42c-4a33-837d-86114f93763e	0	1080d248-b0ca-4124-accd-2ba2f0d30cba	1f981aae-f40b-4dba-b383-8853d87b23c5	1f981aae-f40b-4dba-b383-8853d87b23c5	2024-10-27 00:14:34.802015	\N	\N	\N	\N	f
70bec4ea-9461-4007-b168-a658620b0625	2	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	2024-10-27 00:14:34.745007	\N	\N	\N	\N	f
70c31799-dc95-4bc1-a24b-aeecc29c7922	1	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.771089	\N	\N	\N	\N	f
70ecdc25-4d6b-4239-9b35-b4ec2631dbfb	1	6205b8a1-bea3-493c-9793-bce834e1424e	20105f5a-82e0-4763-b12c-7a5ddc9abf83	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-27 00:14:34.7275	\N	\N	\N	\N	f
722d933a-30bd-404c-81df-8fcad42cb753	1	46355e72-1020-4863-8e28-b06e99861b00	35d0da5e-7492-46d3-aaca-17455a353de9	35d0da5e-7492-46d3-aaca-17455a353de9	2024-10-27 00:14:34.706181	\N	\N	\N	\N	f
72bc141f-dd34-477f-a6a1-ff5a4ae154ad	0	8b2e5b51-9894-4dfd-8f19-00158edb8eee	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	2024-10-27 00:14:34.834438	\N	\N	\N	\N	f
72cdb1e3-37bf-4e56-8e85-d0ac20b0eee6	2	63d17d61-1b1a-4dc6-8b70-b993152948b7	612e214e-4fe6-4b17-b9af-8b8b26bf180e	612e214e-4fe6-4b17-b9af-8b8b26bf180e	2024-10-27 00:14:34.741463	\N	\N	\N	\N	f
73c67776-aaad-4cc6-bbba-734caee6696f	2	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	2024-10-27 00:14:34.785263	\N	\N	\N	\N	f
747e8782-dfaf-4964-bb0b-7b664176e2f6	1	d5adf278-9369-4d33-a9dc-9985037ce413	1cc85c40-c092-4bee-adeb-3dc17e304563	1cc85c40-c092-4bee-adeb-3dc17e304563	2024-10-27 00:14:34.87331	\N	\N	\N	\N	f
75040aa8-48eb-4dff-bbf2-6791c725315c	1	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	275ddc93-92b8-419a-ab96-baeb34d89c19	275ddc93-92b8-419a-ab96-baeb34d89c19	2024-10-27 00:14:34.736237	\N	\N	\N	\N	f
76b0c11d-0098-4a20-9bf1-3a8e22a31df5	1	4adce975-4b0b-491e-a984-87c9785e2f62	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.759759	\N	\N	\N	\N	f
7718629a-176d-4f71-85db-b44cee6f2cbe	1	b4230aa6-612a-40fe-9920-d919118dc656	13ba9424-00b3-40a6-92c8-a9426207a2d9	13ba9424-00b3-40a6-92c8-a9426207a2d9	2024-10-27 00:14:34.709514	\N	\N	\N	\N	f
7758b092-c9c9-4705-9523-605603dd307d	1	1080d248-b0ca-4124-accd-2ba2f0d30cba	b6d54f8d-b08c-4f88-9db9-00008875256f	b6d54f8d-b08c-4f88-9db9-00008875256f	2024-10-27 00:14:34.808934	\N	\N	\N	\N	f
77f75185-85c2-403d-8d81-ebd711044a3c	1	fa08b1d2-6fa6-4b96-9738-57d8b2926053	1f981aae-f40b-4dba-b383-8853d87b23c5	1f981aae-f40b-4dba-b383-8853d87b23c5	2024-10-27 00:14:34.763813	\N	\N	\N	\N	f
795a81d1-25df-4f91-8d2a-ada4917bd0d0	1	4adce975-4b0b-491e-a984-87c9785e2f62	33725381-a183-49ef-b723-e70495ff6066	33725381-a183-49ef-b723-e70495ff6066	2024-10-27 00:14:34.757983	\N	\N	\N	\N	f
7a591aaa-e1b3-4c24-9f42-0f5f5378d003	1	aac8af68-5ebc-40bf-a970-4593b5cf4dfe	439c9800-35c9-48ee-8549-9c293a107743	439c9800-35c9-48ee-8549-9c293a107743	2024-10-27 00:14:34.710133	\N	\N	\N	\N	f
7b2d15a5-6062-427d-86dc-81e3de295c72	0	a5c4336f-e13a-4627-8022-4eddc37c3008	07f86036-511f-47d1-b7b7-4543b2eb3303	07f86036-511f-47d1-b7b7-4543b2eb3303	2024-10-27 00:14:34.822247	\N	\N	\N	\N	f
7b5072dc-4905-4255-890c-b2f390d07895	0	63d17d61-1b1a-4dc6-8b70-b993152948b7	bb05cc9c-87a1-4d43-b253-d172e2117ff2	bb05cc9c-87a1-4d43-b253-d172e2117ff2	2024-10-27 00:14:34.857627	\N	\N	\N	\N	f
7c5f8381-a991-4252-b812-32fa4a6884c7	0	0f087e27-7a21-45ea-b6d3-7296c5360cc3	b6663ea1-57ec-4c3a-9597-da421b3c9484	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-27 00:14:34.774782	\N	\N	\N	\N	f
7cb37e45-3c76-40bc-828d-717a44ba32a4	1	d4d1f3d5-5706-4306-8f06-6065b730b378	b1469423-4113-490e-bcd6-b79a146c3f81	b1469423-4113-490e-bcd6-b79a146c3f81	2024-10-27 00:14:34.711423	\N	\N	\N	\N	f
7e1f1749-b43b-4c95-8d4c-9cf41531342f	2	7c3f3594-0276-4e84-98d3-fe51af9231a8	8b92673a-ba81-4629-aea9-41444a46a0dc	8b92673a-ba81-4629-aea9-41444a46a0dc	2024-10-27 00:14:34.704842	\N	\N	\N	\N	f
7f16e2ae-38ed-4846-baf7-120aea6b807c	2	c9c3aaef-a556-4a26-8ac9-53e4100836a6	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.855122	\N	\N	\N	\N	f
7ff3c1f2-550e-4e0a-9cd3-90d386504d25	0	4afa6810-05d3-4dd0-88e6-776f525ad68b	6700632c-6c3b-4d7e-81dd-8b2151b60502	6700632c-6c3b-4d7e-81dd-8b2151b60502	2024-10-27 00:14:34.878789	\N	\N	\N	\N	f
81837840-380e-4d84-8bc3-fef293a48163	0	a2fecc98-afa0-43da-b6fb-cf7f51585c54	d1372bba-be85-473c-8086-02a7c9890140	d1372bba-be85-473c-8086-02a7c9890140	2024-10-27 00:14:34.895678	\N	\N	\N	\N	f
839e9971-1f01-4d86-aa61-5d02c3bb16c7	0	164a612d-5213-4dca-983e-4a422233dcbe	18e845d8-400b-4d12-a414-9cd440f92677	18e845d8-400b-4d12-a414-9cd440f92677	2024-10-27 00:14:34.82158	\N	\N	\N	\N	f
83d84c6f-c920-47f5-a79d-c980a2786cb0	2	b9d02690-cab8-42e7-89f9-354ae00e25cf	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.894462	\N	\N	\N	\N	f
84fb372e-9996-4017-a9f0-fc9e204b1273	2	0f087e27-7a21-45ea-b6d3-7296c5360cc3	74d9ea46-5729-454f-b994-8faee093ddef	74d9ea46-5729-454f-b994-8faee093ddef	2024-10-27 00:14:34.813771	\N	\N	\N	\N	f
85785c9f-745c-40dc-afff-a74c4d6f214b	1	4adce975-4b0b-491e-a984-87c9785e2f62	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.882695	\N	\N	\N	\N	f
85d23f13-f161-402b-9a2c-ecab7554dadb	1	aac8af68-5ebc-40bf-a970-4593b5cf4dfe	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-27 00:14:34.827309	\N	\N	\N	\N	f
87121885-c9b3-492c-8f63-41a95024b0bd	0	45323ad6-6ddc-436f-aca9-d4e84de1e7b0	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.77826	\N	\N	\N	\N	f
88e81d1e-10ea-4f9c-a813-9c236cf7bcf9	1	5c262361-c160-49b8-818f-8044391f0efb	143437a3-503e-4e95-911d-4c6571ddea8e	143437a3-503e-4e95-911d-4c6571ddea8e	2024-10-27 00:14:34.817759	\N	\N	\N	\N	f
8c1ea944-bbb5-4b00-8801-563c0587bc7f	1	b4230aa6-612a-40fe-9920-d919118dc656	8f722abd-0123-4494-b71c-a21943484a3c	8f722abd-0123-4494-b71c-a21943484a3c	2024-10-27 00:14:34.832578	\N	\N	\N	\N	f
8c863b58-e3b8-485c-a538-f157f09fe42e	0	407ddd4b-c486-4e07-9276-258ef56f6bc9	ed964db3-afac-426e-8988-c2ed54a89510	ed964db3-afac-426e-8988-c2ed54a89510	2024-10-27 00:14:34.786414	\N	\N	\N	\N	f
8ca08b19-672a-4d8f-8144-6f8453eacbf9	1	7f3769d7-35c7-405a-a6e4-cae832a0d30b	35d0da5e-7492-46d3-aaca-17455a353de9	35d0da5e-7492-46d3-aaca-17455a353de9	2024-10-27 00:14:34.756833	\N	\N	\N	\N	f
8de4e862-29e4-49a0-ad04-64396517dfa7	0	164a612d-5213-4dca-983e-4a422233dcbe	3652e96a-9dc0-4f12-817c-1ca7f05807e6	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2024-10-27 00:14:34.830684	\N	\N	\N	\N	f
8ed49f83-40a1-47ab-bd7b-69d23657afff	1	63d17d61-1b1a-4dc6-8b70-b993152948b7	fa846317-fe54-4f52-b8d6-6a618387a5da	fa846317-fe54-4f52-b8d6-6a618387a5da	2024-10-27 00:14:34.790424	\N	\N	\N	\N	f
8fd59110-da9b-4773-bda1-133ea2db10f4	2	f2bceb45-a945-4eef-80b5-e93128060efb	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	2024-10-27 00:14:34.820921	\N	\N	\N	\N	f
9059c004-2ed0-4d41-a4ea-919916094f64	0	a3eaf222-b433-4e57-b143-292939df7d38	275ddc93-92b8-419a-ab96-baeb34d89c19	275ddc93-92b8-419a-ab96-baeb34d89c19	2024-10-27 00:14:34.871346	\N	\N	\N	\N	f
916b0b72-ff43-4826-b3e9-10593c239229	0	1e651773-aa16-4035-a02c-971eb4189198	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.731966	\N	\N	\N	\N	f
922dac25-bae3-4c6c-97de-91404064c5f2	1	d5adf278-9369-4d33-a9dc-9985037ce413	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.712767	\N	\N	\N	\N	f
93dd3d81-5e36-491b-9a41-9ce9eba1b1a7	0	4afa6810-05d3-4dd0-88e6-776f525ad68b	959b7d55-62bf-42c0-a313-33054551abb5	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-27 00:14:34.705558	\N	\N	\N	\N	f
9670fd2a-80bc-4bc1-99bd-7eb1c4dd1fdb	1	424afdfe-46e5-461c-8019-f3e5966607a4	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	2024-10-27 00:14:34.728104	\N	\N	\N	\N	f
9691d7ee-81d2-4bca-b7c3-18de2ce9a700	1	28513cdd-2a9b-43c0-86ef-ff79ee4abff7	2e6b7127-5e54-43eb-a21f-64c57143824d	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-27 00:14:34.700508	\N	\N	\N	\N	f
978b0639-6700-4cdf-a9d2-0ff99e717366	1	c98ac12c-6729-4831-bcb3-473d4cb98a8f	09f405ed-f0c6-422c-847f-0e24f7c74aef	09f405ed-f0c6-422c-847f-0e24f7c74aef	2024-10-27 00:14:34.704127	\N	\N	\N	\N	f
98db5327-1acd-40e9-bdb1-337bae6182c4	2	1e651773-aa16-4035-a02c-971eb4189198	78532cb2-f350-4c98-bce2-e94afd8369c6	78532cb2-f350-4c98-bce2-e94afd8369c6	2024-10-27 00:14:34.77419	\N	\N	\N	\N	f
9fcedcd6-657a-4ecd-8b87-407c8115ff00	0	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	2024-10-27 00:14:34.70238	\N	\N	\N	\N	f
a0d831a6-711f-442d-bb10-3c6f0f0cf61b	0	a2fecc98-afa0-43da-b6fb-cf7f51585c54	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-27 00:14:34.86726	\N	\N	\N	\N	f
a2b629f0-5ead-4d8d-8b10-f341c4d99380	2	45323ad6-6ddc-436f-aca9-d4e84de1e7b0	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.771697	\N	\N	\N	\N	f
a34b970b-3ee1-44fa-84c1-901d76ea9d63	1	b9d02690-cab8-42e7-89f9-354ae00e25cf	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.789212	\N	\N	\N	\N	f
a37f4369-1217-48ec-8534-bb79954e3675	1	5c262361-c160-49b8-818f-8044391f0efb	00c05513-4129-4aa6-b79e-983ff13574fc	00c05513-4129-4aa6-b79e-983ff13574fc	2024-10-27 00:14:34.841818	\N	\N	\N	\N	f
a4a7fb2e-05a7-44e2-bdde-6f7ba43afbc0	2	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.813071	\N	\N	\N	\N	f
a62d0421-8926-421b-ae44-664df8f38de8	1	45323ad6-6ddc-436f-aca9-d4e84de1e7b0	b1469423-4113-490e-bcd6-b79a146c3f81	b1469423-4113-490e-bcd6-b79a146c3f81	2024-10-27 00:14:34.720177	\N	\N	\N	\N	f
a693982b-5c1c-4f17-abad-c8c9e3e19d6f	1	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	78532cb2-f350-4c98-bce2-e94afd8369c6	78532cb2-f350-4c98-bce2-e94afd8369c6	2024-10-27 00:14:34.757404	\N	\N	\N	\N	f
a7293a4f-e743-45f9-9bb1-64a8384b9f67	1	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	f015b253-2d06-44b2-8f52-1ae49c1a241c	f015b253-2d06-44b2-8f52-1ae49c1a241c	2024-10-27 00:14:34.730586	\N	\N	\N	\N	f
a7833de8-523b-4ebe-9b45-9653f0fca0f8	2	1080d248-b0ca-4124-accd-2ba2f0d30cba	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.849487	\N	\N	\N	\N	f
a7c01735-60af-4af1-945e-69dd7a2fc6bd	0	6a40c092-dbdf-46b3-bf6a-90af37dc2470	35d0da5e-7492-46d3-aaca-17455a353de9	35d0da5e-7492-46d3-aaca-17455a353de9	2024-10-27 00:14:34.758554	\N	\N	\N	\N	f
a93e8d06-e60e-419d-87a9-2d71de2eaaf1	0	4afa6810-05d3-4dd0-88e6-776f525ad68b	53453386-8816-485f-9a08-22c07cf29d22	53453386-8816-485f-9a08-22c07cf29d22	2024-10-27 00:14:34.844986	\N	\N	\N	\N	f
a952f018-bc9c-4629-b3c0-717e00607f9b	0	d4d1f3d5-5706-4306-8f06-6065b730b378	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-27 00:14:34.715161	\N	\N	\N	\N	f
a9d32363-8072-4b3b-b00c-fe6ce2b7a034	0	c93495b4-06cd-4c1c-b15d-1c0afd56eb10	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	2024-10-27 00:14:34.784701	\N	\N	\N	\N	f
abb71fd8-b96a-4b87-b192-39d51e01bcd7	1	4afa6810-05d3-4dd0-88e6-776f525ad68b	eb1b0535-b7f3-430e-b91c-c1feea653f5f	eb1b0535-b7f3-430e-b91c-c1feea653f5f	2024-10-27 00:14:34.814443	\N	\N	\N	\N	f
ad0cc4bc-89ca-41cf-b4a4-6cea4f48181c	0	6a40c092-dbdf-46b3-bf6a-90af37dc2470	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.800548	\N	\N	\N	\N	f
ade8aa0d-daff-420e-abb2-f67b4b966b1e	2	7c3f3594-0276-4e84-98d3-fe51af9231a8	53453386-8816-485f-9a08-22c07cf29d22	53453386-8816-485f-9a08-22c07cf29d22	2024-10-27 00:14:34.860008	\N	\N	\N	\N	f
aee34ac8-22f2-4420-8e48-3428b7499711	2	47117efd-4bb9-4a8e-937d-87e596bef6df	0b996fe8-4582-412b-adfb-6fa402c25bf4	0b996fe8-4582-412b-adfb-6fa402c25bf4	2024-10-27 00:14:34.870004	\N	\N	\N	\N	f
afa083c0-4daa-480a-9e1e-1fbf3e7f72ac	2	b4230aa6-612a-40fe-9920-d919118dc656	49fa1298-7d26-4de1-b197-3005c3d03c0e	49fa1298-7d26-4de1-b197-3005c3d03c0e	2024-10-27 00:14:34.861327	\N	\N	\N	\N	f
b2ffe8cc-1ada-4c6e-8ec4-fb2d2f4a0eff	0	a2fecc98-afa0-43da-b6fb-cf7f51585c54	be26aee1-0512-4e6a-8313-5c18759958a9	be26aee1-0512-4e6a-8313-5c18759958a9	2024-10-27 00:14:34.818482	\N	\N	\N	\N	f
b33b4045-de3d-4332-9f2e-20841c26d931	1	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	9f64a38d-8cdd-4a21-a529-9747a9331998	9f64a38d-8cdd-4a21-a529-9747a9331998	2024-10-27 00:14:34.721507	\N	\N	\N	\N	f
b4ba3799-106b-413d-b230-a07006c72df9	2	c9c3aaef-a556-4a26-8ac9-53e4100836a6	384d21de-6a0f-4c92-b0ef-540ff97079bc	384d21de-6a0f-4c92-b0ef-540ff97079bc	2024-10-27 00:14:34.719512	\N	\N	\N	\N	f
b6da8723-4984-426f-97f0-cde7c8ebf8aa	1	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	e095bbae-68d2-4077-9036-697c526736d4	e095bbae-68d2-4077-9036-697c526736d4	2024-10-27 00:14:34.781805	\N	\N	\N	\N	f
b925fe98-946f-447a-90af-6d96f3352831	2	c98ac12c-6729-4831-bcb3-473d4cb98a8f	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.828617	\N	\N	\N	\N	f
ba467c8b-d8b3-4c62-9143-039e1e360325	1	bfc1dd71-fbf0-4b85-8a91-af1686271a2f	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	2024-10-27 00:14:34.838871	\N	\N	\N	\N	f
bbd6a2bf-7615-46c2-8569-8788498f822f	0	63d17d61-1b1a-4dc6-8b70-b993152948b7	be26aee1-0512-4e6a-8313-5c18759958a9	be26aee1-0512-4e6a-8313-5c18759958a9	2024-10-27 00:14:34.737467	\N	\N	\N	\N	f
bc4bab1f-324c-4dd6-963b-aeb8cc08713f	1	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	22e64c46-97c3-40a7-a4aa-4b11eb838446	22e64c46-97c3-40a7-a4aa-4b11eb838446	2024-10-27 00:14:34.746881	\N	\N	\N	\N	f
bd1f86e0-c549-43e9-92af-26a5d7bc3e7b	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	49fa1298-7d26-4de1-b197-3005c3d03c0e	49fa1298-7d26-4de1-b197-3005c3d03c0e	2024-10-27 00:14:34.703052	\N	\N	\N	\N	f
bd66224a-6267-4537-9d52-0a1d6e97893f	2	977d5f30-4fa5-4639-9601-49467c51c384	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.723351	\N	\N	\N	\N	f
bebce20f-2287-467c-9613-1fc3ebd908c1	0	5c262361-c160-49b8-818f-8044391f0efb	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.830034	\N	\N	\N	\N	f
bf56a760-ccad-468a-8884-a4597b6d6e0a	1	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	2024-10-27 00:14:34.706935	\N	\N	\N	\N	f
c4210acf-257c-4c5b-9dad-87283acf051e	1	6949620d-597a-4447-8572-35430780575b	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	2024-10-27 00:14:34.794769	\N	\N	\N	\N	f
c447c4a9-ff92-4a42-b5b7-9053b83cecef	2	46355e72-1020-4863-8e28-b06e99861b00	53453386-8816-485f-9a08-22c07cf29d22	53453386-8816-485f-9a08-22c07cf29d22	2024-10-27 00:14:34.776527	\N	\N	\N	\N	f
c48ca927-de80-4ff5-b93f-c556a746e7f2	1	02d11521-1d02-4af8-b31a-74fa8bb08778	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.882079	\N	\N	\N	\N	f
c4c046dc-0af2-410d-b9c7-8198830a3ea6	1	b9d02690-cab8-42e7-89f9-354ae00e25cf	53453386-8816-485f-9a08-22c07cf29d22	53453386-8816-485f-9a08-22c07cf29d22	2024-10-27 00:14:34.860699	\N	\N	\N	\N	f
c5b51e5d-dcd2-484b-9857-3b86c49475c8	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	14baebc0-0189-423c-a14c-d62ffe981f63	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-27 00:14:34.883305	\N	\N	\N	\N	f
c61af230-de53-4a34-abaa-18a07a2cc3f8	1	46355e72-1020-4863-8e28-b06e99861b00	b3243d6a-7be2-4c83-8a89-dfd4a1976095	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2024-10-27 00:14:34.817156	\N	\N	\N	\N	f
c7073be7-3c31-4b3f-9502-0adf6dd4e2d0	0	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	134e6153-f93b-4592-8bd7-ae30e9321017	134e6153-f93b-4592-8bd7-ae30e9321017	2024-10-27 00:14:34.767441	\N	\N	\N	\N	f
c77b12cf-abe0-4bbd-b275-99b062e86eb5	1	5c262361-c160-49b8-818f-8044391f0efb	959b7d55-62bf-42c0-a313-33054551abb5	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-27 00:14:34.892252	\N	\N	\N	\N	f
c7aeca84-7b00-410c-b536-5d3e5e8ed8a5	1	0f087e27-7a21-45ea-b6d3-7296c5360cc3	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	2024-10-27 00:14:34.793474	\N	\N	\N	\N	f
c91f4212-390f-42cd-b6ca-79675cbc9efd	1	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	3652e96a-9dc0-4f12-817c-1ca7f05807e6	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2024-10-27 00:14:34.802705	\N	\N	\N	\N	f
c949453e-c505-4fd9-b760-8b21176b5c1c	1	7f4c4163-e047-4f4f-a704-6d8fa187758b	fa846317-fe54-4f52-b8d6-6a618387a5da	fa846317-fe54-4f52-b8d6-6a618387a5da	2024-10-27 00:14:34.868729	\N	\N	\N	\N	f
cb989a1d-d29e-4148-83ce-c7acd54da16f	1	aac8af68-5ebc-40bf-a970-4593b5cf4dfe	33725381-a183-49ef-b723-e70495ff6066	33725381-a183-49ef-b723-e70495ff6066	2024-10-27 00:14:34.717073	\N	\N	\N	\N	f
ccb5e9bf-e4e7-42ba-80bb-1d124d76e154	0	5c262361-c160-49b8-818f-8044391f0efb	49fa1298-7d26-4de1-b197-3005c3d03c0e	49fa1298-7d26-4de1-b197-3005c3d03c0e	2024-10-27 00:14:34.880819	\N	\N	\N	\N	f
ce188a65-8b60-42c4-a60f-7b3e26e14ef9	0	fa08b1d2-6fa6-4b96-9738-57d8b2926053	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.853338	\N	\N	\N	\N	f
d28ae63e-ee18-4d01-8924-27c64720d48f	1	d5adf278-9369-4d33-a9dc-9985037ce413	6c1fa607-dced-475d-9ad2-1e8ede9071d2	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2024-10-27 00:14:34.798656	\N	\N	\N	\N	f
d5ad3fd5-abad-4112-8473-4111a4e757f3	1	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	c6d25490-d32a-410d-be77-5370cc1482a2	c6d25490-d32a-410d-be77-5370cc1482a2	2024-10-27 00:14:34.839473	\N	\N	\N	\N	f
d6085838-c60b-43f2-a7e2-699e608f6497	1	a2fecc98-afa0-43da-b6fb-cf7f51585c54	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.827977	\N	\N	\N	\N	f
d61299c7-1004-43aa-9643-d6d34feafbf9	0	6949620d-597a-4447-8572-35430780575b	1f981aae-f40b-4dba-b383-8853d87b23c5	1f981aae-f40b-4dba-b383-8853d87b23c5	2024-10-27 00:14:34.725482	\N	\N	\N	\N	f
d727ce62-f5a7-4889-81f1-9358f96fae29	1	7f3769d7-35c7-405a-a6e4-cae832a0d30b	78532cb2-f350-4c98-bce2-e94afd8369c6	78532cb2-f350-4c98-bce2-e94afd8369c6	2024-10-27 00:14:34.831338	\N	\N	\N	\N	f
d816ade6-3f4f-460d-ae9b-79ad286751bc	2	02d11521-1d02-4af8-b31a-74fa8bb08778	4929722e-df51-411e-8c00-59955f7d8fd8	4929722e-df51-411e-8c00-59955f7d8fd8	2024-10-27 00:14:34.8545	\N	\N	\N	\N	f
d88b2419-cf47-4ce9-9767-5c0f467771e1	0	1080d248-b0ca-4124-accd-2ba2f0d30cba	134e6153-f93b-4592-8bd7-ae30e9321017	134e6153-f93b-4592-8bd7-ae30e9321017	2024-10-27 00:14:34.841204	\N	\N	\N	\N	f
d9a6f514-cf9a-4b7a-9b7d-e8a8122e45cf	2	b4230aa6-612a-40fe-9920-d919118dc656	f015b253-2d06-44b2-8f52-1ae49c1a241c	f015b253-2d06-44b2-8f52-1ae49c1a241c	2024-10-27 00:14:34.768754	\N	\N	\N	\N	f
da9db7f5-9dac-4933-84aa-903ffab965ad	2	0f087e27-7a21-45ea-b6d3-7296c5360cc3	8f722abd-0123-4494-b71c-a21943484a3c	8f722abd-0123-4494-b71c-a21943484a3c	2024-10-27 00:14:34.862584	\N	\N	\N	\N	f
dabe97be-3503-4134-810c-c23c39161171	0	9eac59a5-af43-4b8a-aa6f-d90dc2e16a0d	18e845d8-400b-4d12-a414-9cd440f92677	18e845d8-400b-4d12-a414-9cd440f92677	2024-10-27 00:14:34.886179	\N	\N	\N	\N	f
db4e5866-c8ff-4611-8077-043829ab7856	1	8d44d803-faa6-495e-a0b3-368d5d3cadb2	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.742637	\N	\N	\N	\N	f
dc08bce8-4a11-4137-b1cf-77f0de55a2f7	2	d4d1f3d5-5706-4306-8f06-6065b730b378	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2024-10-27 00:14:34.738064	\N	\N	\N	\N	f
dc940096-d7bc-4f80-b3c1-ae18c1b4626a	0	aac8af68-5ebc-40bf-a970-4593b5cf4dfe	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-27 00:14:34.789837	\N	\N	\N	\N	f
dd94733c-7c85-4aaa-834d-5fd939cf1290	1	55dafc1a-b3af-4ca1-9b61-c2a32f8ac760	959b7d55-62bf-42c0-a313-33054551abb5	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-27 00:14:34.892926	\N	\N	\N	\N	f
ddd1b96b-ff85-43f5-9c7b-11e3f228627c	2	5c262361-c160-49b8-818f-8044391f0efb	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2024-10-27 00:14:34.763231	\N	\N	\N	\N	f
ddefdb76-68ce-4032-bd94-5b0d9f54c28b	2	06314656-9772-4193-b105-edd0c136d72f	72843603-7dc4-4405-92fa-9a7289ac9b66	72843603-7dc4-4405-92fa-9a7289ac9b66	2024-10-27 00:14:34.72214	\N	\N	\N	\N	f
dea085f7-4a84-4967-b03b-bfa6c0cd4a6c	2	d4d1f3d5-5706-4306-8f06-6065b730b378	b6d54f8d-b08c-4f88-9db9-00008875256f	b6d54f8d-b08c-4f88-9db9-00008875256f	2024-10-27 00:14:34.777687	\N	\N	\N	\N	f
e03221f6-2880-46fe-8c18-557f2d444ed5	2	1080d248-b0ca-4124-accd-2ba2f0d30cba	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	2024-10-27 00:14:34.75324	\N	\N	\N	\N	f
e0e6b84f-b934-40fb-b05e-b49fa32004fa	2	5b32a523-4901-477d-99b0-5c2083a1f503	8f722abd-0123-4494-b71c-a21943484a3c	8f722abd-0123-4494-b71c-a21943484a3c	2024-10-27 00:14:34.76034	\N	\N	\N	\N	f
e1cd77bc-1296-4270-a634-73265067bb91	2	63d17d61-1b1a-4dc6-8b70-b993152948b7	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2eb2ae7e-b05a-45c8-83ef-a23717e17947	2024-10-27 00:14:34.768162	\N	\N	\N	\N	f
e2a03234-4857-4065-ab4b-4c27da91a38a	1	b9d02690-cab8-42e7-89f9-354ae00e25cf	09f405ed-f0c6-422c-847f-0e24f7c74aef	09f405ed-f0c6-422c-847f-0e24f7c74aef	2024-10-27 00:14:34.803355	\N	\N	\N	\N	f
e2fca120-bbfc-4760-898f-ffc402d73e42	1	8d44d803-faa6-495e-a0b3-368d5d3cadb2	49fa1298-7d26-4de1-b197-3005c3d03c0e	49fa1298-7d26-4de1-b197-3005c3d03c0e	2024-10-27 00:14:34.840627	\N	\N	\N	\N	f
e30d9246-18ec-4779-8ac3-8ccdc4b08db6	1	ae7f9bb4-1fcd-4b8e-a4a6-e62b1b429f4c	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-27 00:14:34.884763	\N	\N	\N	\N	f
e3fe9329-406f-4f28-b2b7-0626f2b2e57c	2	06314656-9772-4193-b105-edd0c136d72f	6700632c-6c3b-4d7e-81dd-8b2151b60502	6700632c-6c3b-4d7e-81dd-8b2151b60502	2024-10-27 00:14:34.760937	\N	\N	\N	\N	f
e401a523-4357-4d11-80e2-7932c62c8442	2	a2fecc98-afa0-43da-b6fb-cf7f51585c54	09f405ed-f0c6-422c-847f-0e24f7c74aef	09f405ed-f0c6-422c-847f-0e24f7c74aef	2024-10-27 00:14:34.712134	\N	\N	\N	\N	f
e57b84c2-667e-49d5-9f89-1339133ef9f4	0	d4d1f3d5-5706-4306-8f06-6065b730b378	9ca9bcee-c97f-4778-83f4-57fff49759d1	9ca9bcee-c97f-4778-83f4-57fff49759d1	2024-10-27 00:14:34.726169	\N	\N	\N	\N	f
e6d49493-53ad-4e50-92b6-04e8bbc87d3d	0	8e5ffbc4-81dc-4d23-bf51-bbfec5226a13	50088da9-86e5-4781-be1e-f8b04a2554d0	50088da9-86e5-4781-be1e-f8b04a2554d0	2024-10-27 00:14:34.770482	\N	\N	\N	\N	f
e73e1dc6-bd65-4532-944d-42b57c14ff88	2	1e651773-aa16-4035-a02c-971eb4189198	45370c44-1d4d-4834-8cd5-3551b5d30199	45370c44-1d4d-4834-8cd5-3551b5d30199	2024-10-27 00:14:34.783569	\N	\N	\N	\N	f
e7569c07-5d41-4ea8-a6a5-5ebdbc737d5c	2	2a455855-e05d-44d6-96c6-748114b3cb0e	e21d9b47-d1bb-4c02-9704-acff338cf963	e21d9b47-d1bb-4c02-9704-acff338cf963	2024-10-27 00:14:34.755018	\N	\N	\N	\N	f
e7ed5616-d9e9-439c-9473-0b212e9f287e	0	b4230aa6-612a-40fe-9920-d919118dc656	14a6b1d0-f886-4f46-9166-a134668d16ab	14a6b1d0-f886-4f46-9166-a134668d16ab	2024-10-27 00:14:34.885491	\N	\N	\N	\N	f
e92fb642-c511-4d67-96db-1be90f991335	0	c9c3aaef-a556-4a26-8ac9-53e4100836a6	20105f5a-82e0-4763-b12c-7a5ddc9abf83	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-27 00:14:34.878151	\N	\N	\N	\N	f
e99f6408-1eac-473b-bfe6-5bbc150be092	2	a2fecc98-afa0-43da-b6fb-cf7f51585c54	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	2024-10-27 00:14:34.847535	\N	\N	\N	\N	f
eafb7c6b-6f95-4c25-8ea2-a884a80256b4	2	30da2a32-975b-47f3-a347-3a81e4cfd3e4	b6663ea1-57ec-4c3a-9597-da421b3c9484	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-27 00:14:34.876809	\N	\N	\N	\N	f
eb271ac3-6159-4b92-b89a-a55385b39901	0	6205b8a1-bea3-493c-9793-bce834e1424e	14baebc0-0189-423c-a14c-d62ffe981f63	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-27 00:14:34.701466	\N	\N	\N	\N	f
ecbb21bc-897d-4405-9cc9-70a8cf12be87	1	6949620d-597a-4447-8572-35430780575b	e095bbae-68d2-4077-9036-697c526736d4	e095bbae-68d2-4077-9036-697c526736d4	2024-10-27 00:14:34.777105	\N	\N	\N	\N	f
ecc42000-34bf-47ef-863a-0171dc34d3c3	1	18bb351d-3819-40ca-821b-7bbb5ab49883	b55f5bbd-4b95-448a-b38b-a1429002854b	b55f5bbd-4b95-448a-b38b-a1429002854b	2024-10-27 00:14:34.837011	\N	\N	\N	\N	f
edb7b25e-1adb-42dd-a6cf-6686965c154e	1	28513cdd-2a9b-43c0-86ef-ff79ee4abff7	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	2024-10-27 00:14:34.846198	\N	\N	\N	\N	f
eddc6d6e-89a3-4abd-a8be-e0bcb47ad2bf	1	8b2e5b51-9894-4dfd-8f19-00158edb8eee	e00c9a01-ea24-48db-ac41-4d39c79f9321	e00c9a01-ea24-48db-ac41-4d39c79f9321	2024-10-27 00:14:34.707605	\N	\N	\N	\N	f
ef28d2b9-5222-409a-a6cc-17ae0c91907b	1	1e651773-aa16-4035-a02c-971eb4189198	f015b253-2d06-44b2-8f52-1ae49c1a241c	f015b253-2d06-44b2-8f52-1ae49c1a241c	2024-10-27 00:14:34.77595	\N	\N	\N	\N	f
f03faacb-524a-4052-88d5-e3ac62aa4d48	0	7f4c4163-e047-4f4f-a704-6d8fa187758b	ed964db3-afac-426e-8988-c2ed54a89510	ed964db3-afac-426e-8988-c2ed54a89510	2024-10-27 00:14:34.83763	\N	\N	\N	\N	f
f0c2bafb-0ee7-4177-8378-c3d87ff3cd9d	0	d5adf278-9369-4d33-a9dc-9985037ce413	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2024-10-27 00:14:34.76151	\N	\N	\N	\N	f
f0dddebe-e71c-4a44-b5bc-e384d24b86cb	2	4afa6810-05d3-4dd0-88e6-776f525ad68b	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	2024-10-27 00:14:34.74205	\N	\N	\N	\N	f
f187417f-3fe7-41d1-b65e-ffcf72227683	1	5b32a523-4901-477d-99b0-5c2083a1f503	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	2024-10-27 00:14:34.876188	\N	\N	\N	\N	f
f1b104fa-cd08-43fb-bdac-113a133e06c3	2	cc594939-2b4b-478c-aca1-18141715add6	bb05cc9c-87a1-4d43-b253-d172e2117ff2	bb05cc9c-87a1-4d43-b253-d172e2117ff2	2024-10-27 00:14:34.772358	\N	\N	\N	\N	f
f38116bd-5355-4b6d-8f45-8bb567467814	2	64bc7e5d-7b47-44ce-aee3-9a5ac80c2857	439c9800-35c9-48ee-8549-9c293a107743	439c9800-35c9-48ee-8549-9c293a107743	2024-10-27 00:14:34.866588	\N	\N	\N	\N	f
f4b565db-81cc-4be5-9e9e-b550672facb8	2	407ddd4b-c486-4e07-9276-258ef56f6bc9	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2024-10-27 00:14:34.867909	\N	\N	\N	\N	f
f5557c57-d313-4323-93e6-d094f7c4568c	2	5c262361-c160-49b8-818f-8044391f0efb	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.769319	\N	\N	\N	\N	f
f5d256f3-f6e3-4f83-8f81-cf126e52cecc	2	f2bceb45-a945-4eef-80b5-e93128060efb	9612f20e-6fce-4190-bc29-b31d7d3d9188	9612f20e-6fce-4190-bc29-b31d7d3d9188	2024-10-27 00:14:34.738682	\N	\N	\N	\N	f
f6825a2a-ef6d-4f9f-8fde-c52adb361808	1	7f3769d7-35c7-405a-a6e4-cae832a0d30b	978e2b3f-9c26-41f0-b3c4-cba2e492767f	978e2b3f-9c26-41f0-b3c4-cba2e492767f	2024-10-27 00:14:34.887967	\N	\N	\N	\N	f
f71a755e-b333-46b3-bfc5-47bedf99f99a	2	cc594939-2b4b-478c-aca1-18141715add6	6b8b0603-8e07-4181-92ec-ee13f0e768ce	6b8b0603-8e07-4181-92ec-ee13f0e768ce	2024-10-27 00:14:34.786984	\N	\N	\N	\N	f
f7370a9c-b6a1-4092-846f-a905204b9574	2	46355e72-1020-4863-8e28-b06e99861b00	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-27 00:14:34.713363	\N	\N	\N	\N	f
fa8cb5cc-f7a0-4938-a584-a65fc1ce15ec	2	06314656-9772-4193-b105-edd0c136d72f	9612f20e-6fce-4190-bc29-b31d7d3d9188	9612f20e-6fce-4190-bc29-b31d7d3d9188	2024-10-27 00:14:34.877447	\N	\N	\N	\N	f
fc28eecb-f9ab-4f3c-b12e-d612a3d535be	1	b4230aa6-612a-40fe-9920-d919118dc656	3d8be820-f83f-4579-b8e2-a8c4b5465d69	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-27 00:14:34.736855	\N	\N	\N	\N	f
\.


--
-- TOC entry 2842 (class 2606 OID 16898)
-- Name: groups PK_groups; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups
    ADD CONSTRAINT "PK_groups" PRIMARY KEY (id);


--
-- TOC entry 2845 (class 2606 OID 16903)
-- Name: groups_members PK_groups_members; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups_members
    ADD CONSTRAINT "PK_groups_members" PRIMARY KEY (id);


--
-- TOC entry 2843 (class 1259 OID 16909)
-- Name: IX_groups_members_group_id_user_profile_id_created_by; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_groups_members_group_id_user_profile_id_created_by" ON public.groups_members USING btree (group_id, user_profile_id, created_by);


--
-- TOC entry 2846 (class 2606 OID 16904)
-- Name: groups_members FK_groups_members_groups_group_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups_members
    ADD CONSTRAINT "FK_groups_members_groups_group_id" FOREIGN KEY (group_id) REFERENCES public.groups(id) ON DELETE CASCADE;


-- Completed on 2024-10-26 17:21:30 UTC

--
-- PostgreSQL database dump complete
--

