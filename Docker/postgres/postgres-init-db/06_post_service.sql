--
-- PostgreSQL database dump
--

-- Dumped from database version 12.20 (Debian 12.20-1.pgdg110+1)
-- Dumped by pg_dump version 12.20

-- Started on 2024-10-19 15:58:51 UTC
\c post_service_db

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
-- TOC entry 2 (class 3079 OID 16426)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 2982 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 204 (class 1259 OID 16637)
-- Name: posts; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.posts (
    id uuid NOT NULL,
    content text,
    privacy integer NOT NULL,
    post_type integer NOT NULL,
    presentation_id uuid,
    parent_id uuid,
    owner_id uuid NOT NULL,
    group_id uuid,
    media_id uuid,
    created_by uuid,
    created_at timestamp without time zone NOT NULL,
    updated_by uuid,
    updated_at timestamp without time zone,
    deleted_by uuid,
    deleted_at timestamp without time zone,
    is_deleted boolean NOT NULL
);

ALTER TABLE public.posts OWNER TO "infinitynetUser";

--
-- TOC entry 2976 (class 0 OID 16637)
-- Dependencies: 204
-- Data for Name: posts; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.posts (id, content, privacy, post_type, presentation_id, parent_id, owner_id, group_id, media_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
01a7b9f6-5c0e-4978-98e8-9799436f2219	Voluptatem consequuntur occaecati similique voluptas officia ut.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.833424	\N	\N	\N	\N	f
0c323de7-e152-4526-8277-568e8ac7bbf9	Temporibus in fugiat consequatur ipsa quam.	0	1	\N	\N	7b42cb26-668a-4037-8ffc-68058704a460	\N	\N	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 22:16:16.832816	\N	\N	\N	\N	f
0ec69c28-b4d4-481b-b036-34e8e949c75b	Deleniti ut dicta.	0	1	\N	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:16.833928	\N	\N	\N	\N	f
101bf477-c7cf-4214-bfcf-290c6ededae7	Impedit eveniet velit qui magnam ullam qui eaque.	0	1	\N	\N	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	\N	\N	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 22:16:16.833891	\N	\N	\N	\N	f
11661088-169b-469c-a5f7-d262d850f411	Ipsum aspernatur quaerat id.	0	1	\N	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:16.833577	\N	\N	\N	\N	f
118c3d74-9b2c-4deb-9167-a078b3359c09	Consequatur facilis hic quia libero est inventore earum recusandae.	0	1	\N	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	\N	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:16.833864	\N	\N	\N	\N	f
12e7be20-4901-4b1c-b5e8-9ec296a51652	Rerum omnis vel et.	0	1	\N	\N	959b7d55-62bf-42c0-a313-33054551abb5	\N	\N	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:16.833566	\N	\N	\N	\N	f
142bc5b3-8ea7-4f43-ae47-734ec5baef9a	Quisquam ut fuga culpa quam accusantium nam.	0	1	\N	\N	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	\N	\N	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 22:16:16.833119	\N	\N	\N	\N	f
16c628fd-72df-405c-b9f3-c67ccfe2ca36	Aut molestiae eaque natus delectus.	0	1	\N	\N	1f981aae-f40b-4dba-b383-8853d87b23c5	\N	\N	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 22:16:16.832934	\N	\N	\N	\N	f
1b750b2e-efcc-4cee-bcfe-88b2fbf84f18	Ipsa animi qui et blanditiis minus.	0	1	\N	\N	6e132241-d674-4195-b8c5-b6b4d320e3ce	\N	\N	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 22:16:16.83363	\N	\N	\N	\N	f
20e9f107-6942-4de9-b3c7-48fc85b81d98	Expedita aut natus vitae explicabo pariatur mollitia.	0	1	\N	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:16.833511	\N	\N	\N	\N	f
238d8aca-b2ef-437c-a8af-9befaa9aab72	Illo vel voluptas debitis fugiat.	0	1	\N	\N	f015b253-2d06-44b2-8f52-1ae49c1a241c	\N	\N	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 22:16:16.833364	\N	\N	\N	\N	f
275cc92f-a3c8-4c27-b725-86084476f9d0	Quo quidem non mollitia ipsum facilis.	0	1	\N	\N	ed964db3-afac-426e-8988-c2ed54a89510	\N	\N	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 22:16:16.83369	\N	\N	\N	\N	f
27fe695f-242c-479d-a17d-f9c7b7c9abab	Laboriosam velit dolores earum voluptatem consequatur consectetur maiores impedit.	0	1	\N	\N	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	\N	\N	69914b7d-a41b-43ff-9419-b86ddc8d5cb1	2024-10-19 22:16:16.833092	\N	\N	\N	\N	f
2c444ce2-2b6b-41b6-8414-abce7486e726	Nemo atque nihil omnis in facilis et.	0	1	\N	\N	439c9800-35c9-48ee-8549-9c293a107743	\N	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:16.83407	\N	\N	\N	\N	f
2d9808ac-5d2e-4c19-ae6e-4adabfe98e90	Illo rerum animi ipsum quibusdam quia architecto.	0	1	\N	\N	39ad1877-9e15-4498-88bb-ef6d617a23d2	\N	\N	7f003833-3d8a-4f3c-9c18-7986180847e4	2024-10-19 22:16:16.832945	\N	\N	\N	\N	f
301fc3ca-9eb5-4f2a-a1cf-d0cf7e4d3c98	Maiores vel in asperiores officia.	0	1	\N	\N	2e6b7127-5e54-43eb-a21f-64c57143824d	\N	\N	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 22:16:16.832856	\N	\N	\N	\N	f
319fdcb4-3111-4fb1-843b-f09eca2550ca	Perferendis ratione expedita voluptatem beatae cum praesentium dignissimos.	0	1	\N	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	\N	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:16.832922	\N	\N	\N	\N	f
320a585d-d980-4061-96c8-6f6c18370e84	Ipsam qui occaecati odit voluptatem et sunt veniam commodi expedita.	0	1	\N	\N	33725381-a183-49ef-b723-e70495ff6066	\N	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:16.833301	\N	\N	\N	\N	f
376f836a-c287-4c47-8175-9259008a5c6e	Aut voluptates ea reprehenderit dolorum ipsa saepe enim atque aspernatur.	0	1	\N	\N	2fa772f8-0fa4-472b-a154-14cf794d50e2	\N	\N	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 22:16:16.833312	\N	\N	\N	\N	f
39c2d1ab-e652-408c-bf92-c296bcb881e8	Voluptatum ea velit est officiis eius.	0	1	\N	\N	14baebc0-0189-423c-a14c-d62ffe981f63	\N	\N	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:16.834007	\N	\N	\N	\N	f
3a6c11c3-fe68-458d-a590-32eba7e619ef	Eveniet eos dolor voluptatem.	0	1	\N	\N	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	\N	\N	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 22:16:16.834019	\N	\N	\N	\N	f
3d716e90-afdb-4d06-9ba5-c90cebbd1204	Est sed maxime.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	\N	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:16.833951	\N	\N	\N	\N	f
3f472e56-10f9-4a13-89b3-bf38d786c561	Consequatur culpa dignissimos molestiae incidunt nostrum dolores.	0	1	\N	\N	ae5d22bf-3855-460b-a502-9747f35d6a34	\N	\N	f1423b81-e629-47f3-96fd-6fc76e094f58	2024-10-19 22:16:16.833641	\N	\N	\N	\N	f
3f7ebbfa-3e51-4061-a31e-4009511b6aad	Dolore laborum numquam corrupti perspiciatis nostrum qui.	0	1	\N	\N	4929722e-df51-411e-8c00-59955f7d8fd8	\N	\N	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 22:16:16.833014	\N	\N	\N	\N	f
3ff4ac4d-a833-47c5-a2cf-263e7eb0ccc7	Quidem corrupti nulla autem.	0	1	\N	\N	13ba9424-00b3-40a6-92c8-a9426207a2d9	\N	\N	d723eed5-78a1-4fab-9c9d-08efced4b861	2024-10-19 22:16:16.83335	\N	\N	\N	\N	f
4152b866-6163-4820-a307-32308a071fa6	Voluptas cumque quia ex atque aliquam reprehenderit quo facere.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	\N	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:16.833435	\N	\N	\N	\N	f
4303357a-c0c2-46c1-805d-5c8c3d4102fa	Quae magnam laudantium et omnis omnis officia nostrum.	0	1	\N	\N	ed964db3-afac-426e-8988-c2ed54a89510	\N	\N	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 22:16:16.833538	\N	\N	\N	\N	f
448dda8e-53c0-4898-9d41-790dcfec13f1	Et dignissimos quis assumenda animi quasi perferendis maxime ut facere.	0	1	\N	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	\N	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:16.833749	\N	\N	\N	\N	f
457a7fb5-e274-4eb6-acd9-0fadd53440fa	Rerum hic voluptates odio consequatur dolor tempora.	0	1	\N	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	\N	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:16.833187	\N	\N	\N	\N	f
48977d7c-9b80-4c61-8abc-f7559aa0a8f6	Eligendi et et necessitatibus debitis.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.833787	\N	\N	\N	\N	f
48f292b3-2192-492f-a96f-f29088b8df3c	Exercitationem in harum error similique molestiae.	0	1	\N	\N	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	\N	\N	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 22:16:16.832872	\N	\N	\N	\N	f
49e43a2b-2663-433f-b8c3-576a69ef54de	Dolor architecto sit voluptates.	0	1	\N	\N	7374bc88-8afb-4236-9fa0-d75adad253a0	\N	\N	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 22:16:16.833903	\N	\N	\N	\N	f
4a17f2e2-1b1b-4544-a30e-8b267c5adf94	Commodi rerum veniam nulla molestiae pariatur.	0	1	\N	\N	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	\N	\N	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 22:16:16.83321	\N	\N	\N	\N	f
4bbcced6-1d47-4a3a-80dc-f205cf92a1c6	Sit sed veritatis perspiciatis libero adipisci qui perferendis.	0	1	\N	\N	1f981aae-f40b-4dba-b383-8853d87b23c5	\N	\N	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 22:16:16.833733	\N	\N	\N	\N	f
59559abc-4df3-42c0-9d4c-ee0d9fd81d8c	Explicabo vitae similique ea est voluptatem aperiam qui quae quis.	0	1	\N	\N	959b7d55-62bf-42c0-a313-33054551abb5	\N	\N	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:16.832973	\N	\N	\N	\N	f
5b5e7942-1441-4b7a-af62-52b428ea7327	Voluptatum illum similique ipsam corrupti qui.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.833043	\N	\N	\N	\N	f
61a079c7-2810-4d9c-885a-4ec78635af08	Sit et suscipit quae voluptates quisquam.	0	1	\N	\N	3652e96a-9dc0-4f12-817c-1ca7f05807e6	\N	\N	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 22:16:16.83355	\N	\N	\N	\N	f
63da70c9-9ce4-434d-b013-230f9af8a13c	Ut ut cum recusandae ullam.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.83376	\N	\N	\N	\N	f
6649c0f6-58e9-4565-b55c-9ce264140d73	At deleniti aut reprehenderit totam nisi sit occaecati.	0	1	\N	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:16.833284	\N	\N	\N	\N	f
66773bd4-a0bb-43ba-995f-df0e3af42f43	Neque fugit ut nihil enim sapiente optio quis.	0	1	\N	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	\N	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:16.833977	\N	\N	\N	\N	f
6b45fcfd-5bbe-4200-8ac3-adb9f563c449	Totam nemo dolores maiores eum molestias quis sed illum est.	0	1	\N	\N	2eb2ae7e-b05a-45c8-83ef-a23717e17947	\N	\N	bcb42de0-64c2-4e11-890b-7b3de06d0924	2024-10-19 22:16:16.833526	\N	\N	\N	\N	f
74ec5be2-f419-40c4-8981-58aa802f773b	Neque in sit nostrum doloremque facere illum.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	\N	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:16.832909	\N	\N	\N	\N	f
78397937-8e10-41b8-a57a-c4e8edaca3ca	Quas asperiores aut impedit.	0	1	\N	\N	7b42cb26-668a-4037-8ffc-68058704a460	\N	\N	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 22:16:16.833722	\N	\N	\N	\N	f
7b20a1d9-1ec6-4455-93e5-b4d82305eee1	Fugiat voluptatem voluptatem ea.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	\N	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:16.833488	\N	\N	\N	\N	f
7c1f843d-e76a-4f12-bb76-843552976329	Placeat nisi est vel numquam voluptatem.	0	1	\N	\N	33725381-a183-49ef-b723-e70495ff6066	\N	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:16.833772	\N	\N	\N	\N	f
7f6968da-3140-4318-9a21-5279ed9ac9d3	Aut ut eos.	0	1	\N	\N	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	\N	\N	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 22:16:16.83394	\N	\N	\N	\N	f
815276a2-6632-4532-9745-125766c2594a	Pariatur sit nobis eum non.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	\N	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:16.833473	\N	\N	\N	\N	f
823df16c-8d73-4019-a23c-1bc3460d5738	Aut sed nihil consectetur eveniet quas facere.	0	1	\N	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	\N	\N	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:16.833134	\N	\N	\N	\N	f
83e60436-f7bf-48b5-b8d5-c55903214a4d	Non earum ipsum sint temporibus quia blanditiis enim sequi suscipit.	0	1	\N	\N	b6663ea1-57ec-4c3a-9597-da421b3c9484	\N	\N	1adf0cd2-ed45-4722-9875-898a54b06b0b	2024-10-19 22:16:16.833055	\N	\N	\N	\N	f
85701256-203b-4049-8dcb-03bd45a50d9c	Tempora sapiente quia excepturi cupiditate aliquam quas id.	0	1	\N	\N	143437a3-503e-4e95-911d-4c6571ddea8e	\N	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 22:16:16.834056	\N	\N	\N	\N	f
050e0d13-756f-4229-8be7-4e578f8b6017	Sit itaque harum est corrupti quis.	0	1	\N	\N	4929722e-df51-411e-8c00-59955f7d8fd8	682c7de5-825d-4037-b630-0662381923b7	\N	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 22:16:18.124251	\N	\N	\N	\N	f
057fdc9c-9585-4938-9591-b96847735e02	Ea accusamus doloremque soluta ut.	0	1	\N	\N	fadd55dc-c457-41a6-9723-c259182f0035	eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:18.124491	\N	\N	\N	\N	f
092ba746-807a-427f-95fa-ed20f0d93ffc	Alias non consequuntur ullam quis repellendus qui velit velit.	0	1	\N	\N	74d9ea46-5729-454f-b994-8faee093ddef	b3117a33-6280-4bcd-981d-86a87705f58a	\N	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 22:16:18.124426	\N	\N	\N	\N	f
0a0594cf-c807-46a2-b2d5-9ed61693e70e	Aliquid et impedit aut et magnam molestiae voluptatem sit perspiciatis.	0	1	\N	\N	2eb2ae7e-b05a-45c8-83ef-a23717e17947	5310a3e6-5a16-4090-a632-105cae7d42eb	\N	bcb42de0-64c2-4e11-890b-7b3de06d0924	2024-10-19 22:16:18.123331	\N	\N	\N	\N	f
0cea666b-aa08-4b5a-9ad1-b89fcf9a7beb	Cupiditate aliquid quae sint earum deleniti.	0	1	\N	\N	13ba9424-00b3-40a6-92c8-a9426207a2d9	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	\N	d723eed5-78a1-4fab-9c9d-08efced4b861	2024-10-19 22:16:18.12508	\N	\N	\N	\N	f
0e3685d3-febb-4c20-b1d4-151deb7ee693	Voluptas ducimus culpa alias natus labore error optio vel omnis.	0	1	\N	\N	2eb2ae7e-b05a-45c8-83ef-a23717e17947	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	\N	bcb42de0-64c2-4e11-890b-7b3de06d0924	2024-10-19 22:16:18.124186	\N	\N	\N	\N	f
0f392359-0119-47b0-b1dc-b434646eca88	Officia vero architecto et tempore recusandae.	0	1	\N	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	a7c48de9-4698-47cf-ad56-cbaaede24885	\N	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.125124	\N	\N	\N	\N	f
1121680d-259d-48bb-808e-6ca2e0e11965	Nihil id ad pariatur numquam est nisi voluptatem omnis ut.	0	1	\N	\N	439c9800-35c9-48ee-8549-9c293a107743	295c035f-0873-479b-b155-6746e039e598	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.123262	\N	\N	\N	\N	f
17004b29-6677-4f0b-84b1-6a4539216345	Ut voluptatem eum omnis et qui et ipsa omnis ipsam.	0	1	\N	\N	6c1fa607-dced-475d-9ad2-1e8ede9071d2	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	\N	29198ed7-c2be-46cd-a0ed-36bd6a05efbf	2024-10-19 22:16:18.124061	\N	\N	\N	\N	f
188a302b-92cc-494d-b8f8-b0bbc9873ea0	Et adipisci non velit ducimus repudiandae eaque fugiat vero tenetur.	0	1	\N	\N	14baebc0-0189-423c-a14c-d62ffe981f63	82e8961c-1ae8-4912-b4dc-51173b3fdfe6	\N	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:18.124733	\N	\N	\N	\N	f
1a77dc93-b81f-4cfc-ae41-031123ede58f	Est dolorem perferendis rerum iure fugiat et saepe aperiam sit.	0	1	\N	\N	f015b253-2d06-44b2-8f52-1ae49c1a241c	c757d67e-b10d-49f4-b446-3010ae9f9591	\N	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 22:16:18.125255	\N	\N	\N	\N	f
1ca94c5c-7c12-44ad-8b99-e48fb8d709d3	Vitae ut occaecati placeat veritatis earum qui.	0	1	\N	\N	14baebc0-0189-423c-a14c-d62ffe981f63	6f51d38c-087b-4b4c-a69e-edb5b593a401	\N	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:18.123589	\N	\N	\N	\N	f
2111cae2-60b0-42d8-9e6f-f9d39abef4fd	Dignissimos excepturi quis debitis et odit voluptatibus delectus.	0	1	\N	\N	b0d1d45b-c71b-4578-8ac0-01c30b49131b	4dfeb3d3-2c9b-497e-8384-55e94215571d	\N	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 22:16:18.125365	\N	\N	\N	\N	f
2458f419-4b17-47c9-9649-9a3c3d7ae36d	Veritatis qui ut consectetur.	0	1	\N	\N	53453386-8816-485f-9a08-22c07cf29d22	2edf5638-0175-4d58-81fc-92d37118727c	\N	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 22:16:18.124927	\N	\N	\N	\N	f
27307601-1736-4c6e-9a45-a41c9cf62607	Inventore ipsum consectetur doloribus aperiam.	0	1	\N	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	d3935e8e-8abe-46dc-878a-4434da7af9ec	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:18.125429	\N	\N	\N	\N	f
2ce94f37-21a2-4db3-a833-a545735f2869	Deleniti error nesciunt cupiditate sint est.	0	1	\N	\N	fe1e460d-16ac-4fcd-b512-2413b8cb3256	12b52fea-b990-404f-9f77-13d66ec80399	\N	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 22:16:18.124314	\N	\N	\N	\N	f
2de6cc40-8bfa-44d4-8926-e538727bbcaa	Officiis est in velit dolorem.	0	1	\N	\N	00c05513-4129-4aa6-b79e-983ff13574fc	ac641cb5-3883-4fbd-9783-9770175859f1	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:18.124206	\N	\N	\N	\N	f
2e1a6c06-44d5-44e2-9664-3c67eafdf823	Aspernatur molestiae eaque sit eius perspiciatis et voluptas.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.124166	\N	\N	\N	\N	f
3411d2a8-419f-4ec7-bc2c-da7c2e57cb4a	Dignissimos aperiam tempora commodi quo et ratione vel.	0	1	\N	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	26ab939c-c075-43f7-b16e-6a695866d173	\N	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 22:16:18.125409	\N	\N	\N	\N	f
3473d1df-17bd-4908-82e8-e0038baaee5b	Maiores ipsum corrupti ut sed.	0	1	\N	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	d3935e8e-8abe-46dc-878a-4434da7af9ec	\N	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.12447	\N	\N	\N	\N	f
34f1645c-ffad-48eb-8c37-4746bb1b7306	Eos et impedit aliquam totam magni iste incidunt.	0	1	\N	\N	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	40e92b5b-736d-4b63-ad95-7dfe2d26bc04	\N	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 22:16:18.12381	\N	\N	\N	\N	f
353e0882-b850-4050-b1de-fd5ab5553cea	Voluptatem et dolore id neque dicta cumque ut ut qui.	0	1	\N	\N	b1469423-4113-490e-bcd6-b79a146c3f81	b3117a33-6280-4bcd-981d-86a87705f58a	\N	0ecdbfd7-a759-41de-81db-f550960f3f10	2024-10-19 22:16:18.124952	\N	\N	\N	\N	f
35aab92b-6e5c-48ef-86b4-1f0e1254f75a	Repellat alias recusandae delectus id.	0	1	\N	\N	439c9800-35c9-48ee-8549-9c293a107743	5310a3e6-5a16-4090-a632-105cae7d42eb	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.124777	\N	\N	\N	\N	f
3a51e814-55f3-4c4b-bf63-339cbc56e300	Aut sint et provident ab minus accusantium distinctio doloremque possimus.	0	1	\N	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.124669	\N	\N	\N	\N	f
3f187813-80da-4db8-89c6-c29108b3101e	Libero vitae ipsum blanditiis ut numquam.	0	1	\N	\N	1cc85c40-c092-4bee-adeb-3dc17e304563	71691b66-37b0-4a42-95e2-f6d2c14a7d75	\N	3d3cb675-d596-49aa-89af-61479d8c8e8d	2024-10-19 22:16:18.123436	\N	\N	\N	\N	f
41b6bfe1-00ce-49d7-a5cd-606aaf2ce118	Dolore et corrupti non consequatur magni autem.	0	1	\N	\N	00c05513-4129-4aa6-b79e-983ff13574fc	ac641cb5-3883-4fbd-9783-9770175859f1	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:18.124995	\N	\N	\N	\N	f
45adf061-7651-48bc-961f-ad19ad28a780	Neque aut in accusamus dolor consectetur quas.	0	1	\N	\N	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 22:16:18.125166	\N	\N	\N	\N	f
461a8c38-7539-4e60-ac19-23bb39110b86	Qui itaque quae occaecati nihil eveniet omnis.	0	1	\N	\N	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2edf5638-0175-4d58-81fc-92d37118727c	\N	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 22:16:18.12368	\N	\N	\N	\N	f
49b774e5-1942-43f3-a078-8bffad1bf5ce	Eaque reiciendis quo sunt debitis pariatur necessitatibus ad consequuntur est.	0	1	\N	\N	6b8b0603-8e07-4181-92ec-ee13f0e768ce	6b0a3307-50cd-4ab8-b239-52ec9227ff19	\N	41866800-c7ac-46ac-9cc8-a6190d3e47ce	2024-10-19 22:16:18.124511	\N	\N	\N	\N	f
4e75e413-3eb6-49a6-9a22-2b2e017e3bc7	Est ut eum distinctio non et et accusamus.	0	1	\N	\N	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	68151439-6890-4954-9559-06b02c84acdd	\N	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 22:16:18.124339	\N	\N	\N	\N	f
53de4d12-ab1c-42a0-b1fd-06228672fc3f	Et corrupti saepe dolores nihil corrupti assumenda sapiente ullam.	0	1	\N	\N	b6d54f8d-b08c-4f88-9db9-00008875256f	01b040e9-784f-4edc-a439-2996df603eae	\N	120acdc1-8799-412b-8fc8-67addf841f25	2024-10-19 22:16:18.124621	\N	\N	\N	\N	f
568e9391-1e65-4d4d-ab56-ae3355793bcb	Ab ipsa modi earum minus adipisci pariatur velit unde.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	71691b66-37b0-4a42-95e2-f6d2c14a7d75	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.123567	\N	\N	\N	\N	f
5763cded-6ecc-4b57-aee4-0087d269ad88	Sunt nulla placeat.	0	1	\N	\N	22e64c46-97c3-40a7-a4aa-4b11eb838446	682c7de5-825d-4037-b630-0662381923b7	\N	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 22:16:18.123059	\N	\N	\N	\N	f
579815c5-4f4e-42c2-bf68-d7dc61b19f00	Sequi tempore veritatis maxime eveniet quia ea.	0	1	\N	\N	eb1b0535-b7f3-430e-b91c-c1feea653f5f	39796686-5676-4e48-914a-f405fbead580	\N	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 22:16:18.123388	\N	\N	\N	\N	f
589d0473-ec0b-47be-8ee2-d3422cba405d	Nemo nihil et vitae vel ipsam laborum necessitatibus et.	0	1	\N	\N	384d21de-6a0f-4c92-b0ef-540ff97079bc	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 22:16:18.124974	\N	\N	\N	\N	f
59ebf797-2b58-47d6-bbcf-a3b54a0c0d00	Animi soluta aut similique aperiam cum officia quis quaerat.	0	1	\N	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	d3935e8e-8abe-46dc-878a-4434da7af9ec	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.12363	\N	\N	\N	\N	f
5efe6dd4-68af-4db8-b6d1-5047af5405eb	Dolores enim quos velit placeat numquam aut.	0	1	\N	\N	b0d1d45b-c71b-4578-8ac0-01c30b49131b	deb52d19-077b-42d0-8949-2a7826f2c6a1	\N	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 22:16:18.124383	\N	\N	\N	\N	f
5f59de6e-8c5f-4654-986c-edd7ba6cc316	Aperiam provident quod laboriosam animi autem.	0	1	\N	\N	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	39da44d5-76ab-47bc-9ff8-121c965e47d5	\N	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 22:16:18.123745	\N	\N	\N	\N	f
62223d3c-b5ed-4bbb-a5a9-e34200641209	Laborum itaque corrupti voluptatem iste.	0	1	\N	\N	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2edf5638-0175-4d58-81fc-92d37118727c	\N	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 22:16:18.125144	\N	\N	\N	\N	f
6364a0a0-6fd1-4d13-92ee-161ef7a496e6	Debitis aperiam iusto et aut et quibusdam voluptatum.	0	1	\N	\N	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	b3117a33-6280-4bcd-981d-86a87705f58a	\N	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 22:16:18.123659	\N	\N	\N	\N	f
65912a67-50e2-49dc-9dc1-8668f656efb1	Veniam cum quis magnam ut alias mollitia ad rerum odio.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.125058	\N	\N	\N	\N	f
6e5f661e-f280-4ba0-ac28-31fb568a21ca	Temporibus qui ipsa.	0	1	\N	\N	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	12b52fea-b990-404f-9f77-13d66ec80399	\N	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 22:16:18.124882	\N	\N	\N	\N	f
704301bf-b818-4cbe-bcfe-72673a5934a0	Et voluptas enim qui id nulla quia error minus aut.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.124124	\N	\N	\N	\N	f
73d65748-80de-4816-a6e9-c2176cb80e97	Sit officiis soluta tempora sed est ratione dicta.	0	1	\N	\N	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	\N	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 22:16:18.123416	\N	\N	\N	\N	f
01de521c-9e75-480e-a285-00e73856d784	Tenetur debitis veritatis impedit pariatur.	0	1	\N	\N	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	71691b66-37b0-4a42-95e2-f6d2c14a7d75	\N	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 22:16:18.12528	\N	\N	\N	\N	f
03282ed9-1f38-446e-9111-8a2061652e74	Sed dolores eveniet quos perferendis non eligendi dolor.	0	1	\N	\N	d1372bba-be85-473c-8086-02a7c9890140	6b0a3307-50cd-4ab8-b239-52ec9227ff19	\N	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 22:16:18.125385	\N	\N	\N	\N	f
85baf32f-6bda-42b5-92a2-be82a907d954	Placeat repellendus dignissimos.	0	1	\N	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	\N	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:16.833247	\N	\N	\N	\N	f
869a61b9-eae9-4a1a-ad8d-14abf04f2797	Est officiis sunt id ratione dolor dolorum autem.	0	1	\N	\N	ae5d22bf-3855-460b-a502-9747f35d6a34	\N	\N	f1423b81-e629-47f3-96fd-6fc76e094f58	2024-10-19 22:16:16.832776	\N	\N	\N	\N	f
88b1a3ae-1d49-4572-9029-1d3ac0c368e6	Similique fugit itaque.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.834044	\N	\N	\N	\N	f
8a7d2aed-e049-4c46-b827-a6cd9e748d73	Assumenda omnis id dolor minima sit facilis.	0	1	\N	\N	bb05cc9c-87a1-4d43-b253-d172e2117ff2	\N	\N	694020bc-a98b-4a12-93da-c9331c68619a	2024-10-19 22:16:16.833462	\N	\N	\N	\N	f
8c9f93f1-28e6-42e6-a0e9-082f4569bb9f	Vel labore voluptatum non maiores inventore aut saepe porro quis.	0	1	\N	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	\N	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:16.833811	\N	\N	\N	\N	f
902439f5-1aa6-4778-ac4c-980efad02b9a	Error illum animi ipsam.	0	1	\N	\N	00c05513-4129-4aa6-b79e-983ff13574fc	\N	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:16.833593	\N	\N	\N	\N	f
972a6f4e-8f56-41aa-a0e3-366f36b02f06	Tempora dolore quia quam non eveniet.	0	1	\N	\N	b1469423-4113-490e-bcd6-b79a146c3f81	\N	\N	0ecdbfd7-a759-41de-81db-f550960f3f10	2024-10-19 22:16:16.83308	\N	\N	\N	\N	f
9820c4a1-422f-4a45-b9d6-1f351c952bb5	Aut omnis dolores molestiae velit.	0	1	\N	\N	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	\N	\N	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 22:16:16.833407	\N	\N	\N	\N	f
9b6b3869-62e4-4636-8b63-56a5205e5e12	Nihil fugiat perspiciatis voluptatem aut maiores et veritatis quaerat et.	0	1	\N	\N	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	\N	\N	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 22:16:16.833198	\N	\N	\N	\N	f
9c057159-af4c-41bf-883c-711646ccbb73	Est nam voluptas repudiandae.	0	1	\N	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:16.833652	\N	\N	\N	\N	f
a1c1bd06-707b-4de9-b4b2-cf301b2813e8	Molestiae quia aliquid ad enim laborum iure quam.	0	1	\N	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	\N	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:16.834081	\N	\N	\N	\N	f
a881878c-730e-4f8c-9af3-9eb2b6880231	In est ut consequuntur.	0	1	\N	\N	b55f5bbd-4b95-448a-b38b-a1429002854b	\N	\N	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 22:16:16.83371	\N	\N	\N	\N	f
a9846baa-be27-4be6-afee-72fe41d46c82	Quo quia est asperiores aut ea dolor molestias.	0	1	\N	\N	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	\N	\N	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 22:16:16.833324	\N	\N	\N	\N	f
aa6da1b3-2356-41a9-a840-27e598c61590	Qui qui inventore excepturi asperiores.	0	1	\N	\N	e00c9a01-ea24-48db-ac41-4d39c79f9321	\N	\N	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 22:16:16.833261	\N	\N	\N	\N	f
ad0dc67e-662a-4188-bf86-141406219972	Porro fugit ea animi ratione aut voluptate.	0	1	\N	\N	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	\N	\N	b6a3426d-d4da-49e2-b18e-eb40caad3700	2024-10-19 22:16:16.832883	\N	\N	\N	\N	f
adc8b503-25e2-49cd-9afc-dc56d77c5a43	Eum aliquam ut dolores beatae minima qui magnam exercitationem quibusdam.	0	1	\N	\N	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	\N	\N	f8ebf6a1-45d5-4b39-a5fa-4c862867ee36	2024-10-19 22:16:16.833376	\N	\N	\N	\N	f
ae7eecd0-e638-4712-9b96-c725eb979912	Et consequatur voluptate sint.	0	1	\N	\N	eba19f8f-0936-45eb-88bc-9c83772a1d93	\N	\N	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 22:16:16.83315	\N	\N	\N	\N	f
b46a53ca-c87d-4846-980b-c204b79acd0a	Veniam nemo dignissimos velit beatae repudiandae a ea modi autem.	0	1	\N	\N	8b92673a-ba81-4629-aea9-41444a46a0dc	\N	\N	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 22:16:16.833837	\N	\N	\N	\N	f
b5e780f4-f417-4e96-b35d-36cc34ba64ac	At repellendus fuga delectus ut itaque mollitia autem.	0	1	\N	\N	eb1b0535-b7f3-430e-b91c-c1feea653f5f	\N	\N	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 22:16:16.833173	\N	\N	\N	\N	f
b6aed98a-9fcb-4222-af33-35acffae049a	Sed eius doloremque in.	0	1	\N	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	\N	\N	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 22:16:16.833026	\N	\N	\N	\N	f
bf948a6b-3c92-48ba-83ae-9a018f381f15	Quos impedit sit consequuntur aspernatur laboriosam.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	\N	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:16.83345	\N	\N	\N	\N	f
c2a77f28-65d7-4b8c-8da9-b0c50ee076df	Ut ea cupiditate odio ullam.	0	1	\N	\N	fadd55dc-c457-41a6-9723-c259182f0035	\N	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:16.833667	\N	\N	\N	\N	f
c46c8edc-347c-4d66-b9c5-72db6ef5dda5	Voluptatibus iste necessitatibus beatae minima animi odio praesentium eius.	0	1	\N	\N	8b92673a-ba81-4629-aea9-41444a46a0dc	\N	\N	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 22:16:16.833989	\N	\N	\N	\N	f
c5074b4e-1119-402b-87cf-2e65df5b77dc	Et nesciunt maxime architecto.	0	1	\N	\N	9f64a38d-8cdd-4a21-a529-9747a9331998	\N	\N	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 22:16:16.833851	\N	\N	\N	\N	f
c51dd2e7-091c-46e2-a2d5-571ecfa07202	Et voluptatem quis et.	0	1	\N	\N	fadd55dc-c457-41a6-9723-c259182f0035	\N	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:16.833002	\N	\N	\N	\N	f
c529d666-1da3-4423-9c10-9f75b6299e82	Saepe reiciendis debitis porro itaque earum id facilis.	0	1	\N	\N	2e6b7127-5e54-43eb-a21f-64c57143824d	\N	\N	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 22:16:16.833224	\N	\N	\N	\N	f
cb08a08d-6503-44d1-bcb8-5e87154d80ab	Qui nemo quo sint assumenda pariatur nemo reprehenderit.	0	1	\N	\N	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	\N	\N	f8ebf6a1-45d5-4b39-a5fa-4c862867ee36	2024-10-19 22:16:16.833338	\N	\N	\N	\N	f
cc892919-9054-4b04-8dd4-b7db10779664	Optio aspernatur non non ipsa quo rerum amet.	0	1	\N	\N	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	\N	\N	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 22:16:16.833679	\N	\N	\N	\N	f
cd32c9c6-8f6d-4dd1-8f04-b0999044b264	Iure exercitationem quaerat maxime tempora dignissimos odit est.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	\N	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:16.833615	\N	\N	\N	\N	f
cf1edd7a-0025-412f-a4bc-0a04b295626f	Ut porro suscipit est nostrum.	0	1	\N	\N	50088da9-86e5-4781-be1e-f8b04a2554d0	\N	\N	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:16.833235	\N	\N	\N	\N	f
daab4c04-b414-46d6-a837-4eed074ff221	Nesciunt maxime enim qui sunt.	0	1	\N	\N	e00c9a01-ea24-48db-ac41-4d39c79f9321	\N	\N	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 22:16:16.8335	\N	\N	\N	\N	f
deee2445-0b40-4299-adfc-c417f00aa7c7	Occaecati recusandae temporibus tempora.	0	1	\N	\N	6c1fa607-dced-475d-9ad2-1e8ede9071d2	\N	\N	29198ed7-c2be-46cd-a0ed-36bd6a05efbf	2024-10-19 22:16:16.833914	\N	\N	\N	\N	f
e08ace5f-c6f8-452f-9625-c78299b32857	A aut nemo magnam reprehenderit hic.	0	1	\N	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	\N	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:16.832984	\N	\N	\N	\N	f
e1b0cd53-6e8c-41f4-8c52-4c58d67cdc06	Ut pariatur consequatur fugiat quis soluta et.	0	1	\N	\N	eba19f8f-0936-45eb-88bc-9c83772a1d93	\N	\N	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 22:16:16.833273	\N	\N	\N	\N	f
e2912da0-31ba-4a3a-8d12-a6b192e32ddf	Neque illum porro ducimus minima dolor possimus quam aliquam.	0	1	\N	\N	d1372bba-be85-473c-8086-02a7c9890140	\N	\N	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 22:16:16.833387	\N	\N	\N	\N	f
e34691fd-3439-40b7-ba62-c7526086e0ee	Porro ab nulla officiis est adipisci ab vel.	0	1	\N	\N	134e6153-f93b-4592-8bd7-ae30e9321017	\N	\N	dc2623fe-8a17-4340-abf9-d51a6e118efc	2024-10-19 22:16:16.832802	\N	\N	\N	\N	f
e59d69ee-525a-470f-9559-a02901df71cc	Officiis corrupti id reiciendis illo id.	0	1	\N	\N	b1469423-4113-490e-bcd6-b79a146c3f81	\N	\N	0ecdbfd7-a759-41de-81db-f550960f3f10	2024-10-19 22:16:16.8338	\N	\N	\N	\N	f
e6731f03-7699-4820-ad24-ff7f662748f1	Nisi in voluptatibus sed dolor distinctio.	0	1	\N	\N	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	\N	\N	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 22:16:16.832961	\N	\N	\N	\N	f
e7a606de-db6b-4af9-8469-cc5e6132c119	Qui eum odit sed nostrum dolor.	0	1	\N	\N	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	\N	\N	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 22:16:16.833604	\N	\N	\N	\N	f
eb26d5e7-be4d-4d5b-b43c-189a9295f9c2	Iure qui natus.	0	1	\N	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	\N	\N	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:16.833104	\N	\N	\N	\N	f
ed193d73-004e-4abc-a93a-8d4b18aa82e0	Porro qui necessitatibus recusandae nihil nisi eaque sint rerum sunt.	0	1	\N	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	\N	\N	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 22:16:16.832897	\N	\N	\N	\N	f
eddb14fa-3c59-43cf-9e09-9f286d3e1376	Voluptas numquam ut maiores fuga laborum.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	\N	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:16.831308	\N	\N	\N	\N	f
ee1015a1-6246-40be-8820-52abbd0419dc	Soluta dolor harum aut natus eum accusamus quasi.	0	1	\N	\N	978e2b3f-9c26-41f0-b3c4-cba2e492767f	\N	\N	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 22:16:16.832828	\N	\N	\N	\N	f
ef74ddb1-33fb-49fc-b1fe-0863f4f643c5	Esse et dolorem omnis.	0	1	\N	\N	384d21de-6a0f-4c92-b0ef-540ff97079bc	\N	\N	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 22:16:16.83403	\N	\N	\N	\N	f
f004d9c3-5999-4725-8d97-dd7953416005	Ducimus consequuntur iusto in consequatur.	0	1	\N	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	\N	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:16.833825	\N	\N	\N	\N	f
f0714bda-b44a-4e9a-a30b-821a035943be	Tenetur eligendi aut.	0	1	\N	\N	b0d1d45b-c71b-4578-8ac0-01c30b49131b	\N	\N	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 22:16:16.833161	\N	\N	\N	\N	f
f393b944-bc5c-4f3f-b3fd-355170e17a06	Porro beatae pariatur exercitationem debitis omnis voluptates qui.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	\N	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:16.833066	\N	\N	\N	\N	f
fbf6561e-a09c-4608-a3fa-873a2026a921	Enim quia blanditiis qui commodi deleniti.	0	1	\N	\N	50088da9-86e5-4781-be1e-f8b04a2554d0	\N	\N	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:16.833876	\N	\N	\N	\N	f
fc507390-8c6e-404f-a09f-7a76385f3a1a	Quis eos officiis soluta.	0	1	\N	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	\N	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:16.833966	\N	\N	\N	\N	f
75957e1d-8200-43a4-ad6e-0a6bb6ab34ce	Rerum omnis aliquam harum quia animi quos cumque at.	0	1	\N	\N	74d9ea46-5729-454f-b994-8faee093ddef	b3117a33-6280-4bcd-981d-86a87705f58a	\N	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 22:16:18.1235	\N	\N	\N	\N	f
7967affa-75e5-42d9-be9c-9616e26a29c7	Ipsam sint odio sed.	0	1	\N	\N	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	\N	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 22:16:18.123521	\N	\N	\N	\N	f
7a57a171-f2dd-4008-aa27-bd400e24cadd	Ut delectus voluptatem qui iusto quae.	0	1	\N	\N	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	ac641cb5-3883-4fbd-9783-9770175859f1	\N	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 22:16:18.124601	\N	\N	\N	\N	f
7a93e6e0-7225-4bca-9865-96108b806aed	Voluptas sit dolorum dolor omnis.	0	1	\N	\N	84609dec-b050-496e-81be-301a1334919a	32a4c31c-5048-43d8-b8e4-5734f0f6741c	\N	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 22:16:18.124012	\N	\N	\N	\N	f
7b0c1c45-6de6-4c60-b2df-b2488dae411e	Nihil non rerum eum cumque ipsum debitis architecto sequi.	0	1	\N	\N	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	38e635f2-1b68-473b-b941-1e81d253578f	\N	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 22:16:18.124647	\N	\N	\N	\N	f
85927a7a-a8e4-4a7e-9a1f-5526d56e5440	Sit vitae suscipit asperiores est ab.	0	1	\N	\N	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	d3935e8e-8abe-46dc-878a-4434da7af9ec	\N	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 22:16:18.123207	\N	\N	\N	\N	f
8824c253-9f54-4774-a138-ca72078a4892	Nihil nam qui et eos.	0	1	\N	\N	00c05513-4129-4aa6-b79e-983ff13574fc	e33d90bd-73e3-4002-9da2-db6d1180de0e	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:18.12395	\N	\N	\N	\N	f
8a23beaa-b3c4-4396-91d3-2fb44888d595	Qui sit consequatur eaque ullam odio.	0	1	\N	\N	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	ac641cb5-3883-4fbd-9783-9770175859f1	\N	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 22:16:18.124904	\N	\N	\N	\N	f
8be9cc31-3a14-44ad-b689-9d7c01c79fcc	Aut qui odio voluptas.	0	1	\N	\N	33725381-a183-49ef-b723-e70495ff6066	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:18.125301	\N	\N	\N	\N	f
92779b12-d721-4bd2-8dec-3d4c4f0ea44e	Voluptatibus voluptas cumque amet error magnam.	0	1	\N	\N	3652e96a-9dc0-4f12-817c-1ca7f05807e6	add17a7a-8cdd-47f2-ab30-e237b54ba751	\N	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 22:16:18.123725	\N	\N	\N	\N	f
957d3a36-b163-4d86-8922-039e7b3d4da8	Deserunt tenetur placeat quasi beatae quidem quae.	0	1	\N	\N	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	12b52fea-b990-404f-9f77-13d66ec80399	\N	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 22:16:18.12521	\N	\N	\N	\N	f
96a9e80b-01fa-4df2-8f0e-a1d4656f69b3	Suscipit voluptatibus unde nesciunt id perspiciatis ut non.	0	1	\N	\N	35d0da5e-7492-46d3-aaca-17455a353de9	77bedc14-cd28-4fd7-8550-90cab9470ba4	\N	80c16f07-671b-472d-be58-e5fd82bedce0	2024-10-19 22:16:18.123547	\N	\N	\N	\N	f
96c78335-23c5-446a-a0d0-cd3cf753553b	Accusantium delectus rem pariatur sit et.	0	1	\N	\N	74d9ea46-5729-454f-b994-8faee093ddef	deb52d19-077b-42d0-8949-2a7826f2c6a1	\N	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 22:16:18.125103	\N	\N	\N	\N	f
971ba10f-e770-4631-846d-a0ba3f898364	Nisi occaecati quia sit voluptas officiis ratione ipsa et.	0	1	\N	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.124819	\N	\N	\N	\N	f
9728beea-4734-43d2-8d67-173533e8a3c4	Sunt et rerum.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	095f9791-d050-4330-906b-0647ea1786f4	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.12469	\N	\N	\N	\N	f
995bf197-b6e7-4f5f-8d85-369bac021073	Reprehenderit voluptas sit maiores repudiandae recusandae tempora.	0	1	\N	\N	50088da9-86e5-4781-be1e-f8b04a2554d0	5310a3e6-5a16-4090-a632-105cae7d42eb	\N	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:18.124104	\N	\N	\N	\N	f
9a206336-e32e-4cb9-9127-b3e095a65b13	Illo pariatur corporis.	0	1	\N	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	c757d67e-b10d-49f4-b446-3010ae9f9591	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.124578	\N	\N	\N	\N	f
9dac6413-22d2-4a94-a37d-d58848d34e6c	Et sapiente ut vel aut saepe quia quia.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	38e635f2-1b68-473b-b941-1e81d253578f	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.124081	\N	\N	\N	\N	f
9ed79ce4-2da8-42a7-aa3f-22cf7ea87102	Nam eos praesentium est sed enim.	0	1	\N	\N	50088da9-86e5-4781-be1e-f8b04a2554d0	4dfeb3d3-2c9b-497e-8384-55e94215571d	\N	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:18.124231	\N	\N	\N	\N	f
9ed7e9b2-6df8-4983-8aca-59b20c38f6f0	Modi est aut doloribus est.	0	1	\N	\N	143437a3-503e-4e95-911d-4c6571ddea8e	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 22:16:18.124862	\N	\N	\N	\N	f
9fd8d421-2c46-4073-8886-e6d32d44c122	Possimus blanditiis cumque est et possimus aut perspiciatis nostrum.	0	1	\N	\N	fadd55dc-c457-41a6-9723-c259182f0035	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:18.124754	\N	\N	\N	\N	f
a229dba8-e286-4046-97bc-0281dad42e56	Quia ullam voluptatem beatae.	0	1	\N	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	a7c48de9-4698-47cf-ad56-cbaaede24885	\N	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.125038	\N	\N	\N	\N	f
a68a3d70-b3d8-4ed2-aaa3-4ff392b4e88f	Modi asperiores in maxime fuga sint sit voluptatem dolor.	0	1	\N	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	cef12cfc-5e1d-42c4-b7a0-90b57b0e2a36	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:18.124557	\N	\N	\N	\N	f
a6e96bc1-3e81-4cea-bb0d-3664a1ae434d	Enim voluptas incidunt totam alias non.	0	1	\N	\N	33725381-a183-49ef-b723-e70495ff6066	2edf5638-0175-4d58-81fc-92d37118727c	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:18.123178	\N	\N	\N	\N	f
a9565a54-bc57-4e3c-b2f2-f4a35c437db8	Ea illum consequuntur laborum facilis quam ab aspernatur.	0	1	\N	\N	49fa1298-7d26-4de1-b197-3005c3d03c0e	c11966e2-ee59-4f31-9807-791e2e1c9a3d	\N	88ea9d8d-9bf0-40ed-a794-32835eac461a	2024-10-19 22:16:18.124798	\N	\N	\N	\N	f
aace8630-9cdb-40aa-a084-d6a0c5e0b8ed	Necessitatibus occaecati nihil.	0	1	\N	\N	8b92673a-ba81-4629-aea9-41444a46a0dc	4dfeb3d3-2c9b-497e-8384-55e94215571d	\N	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 22:16:18.124362	\N	\N	\N	\N	f
b380a257-4d78-4847-b999-039ea1485a91	Exercitationem cum et et ex nemo et vel libero.	0	1	\N	\N	f015b253-2d06-44b2-8f52-1ae49c1a241c	c757d67e-b10d-49f4-b446-3010ae9f9591	\N	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 22:16:18.124271	\N	\N	\N	\N	f
b62ec2e6-6c4f-4088-939d-5b23fa1e1347	Voluptatibus dolores modi.	0	1	\N	\N	53453386-8816-485f-9a08-22c07cf29d22	2edf5638-0175-4d58-81fc-92d37118727c	\N	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 22:16:18.123307	\N	\N	\N	\N	f
b7cbc435-c3da-4737-a8b2-672513fb825e	Magni ad ut magnam aperiam cum.	0	1	\N	\N	84609dec-b050-496e-81be-301a1334919a	32a4c31c-5048-43d8-b8e4-5734f0f6741c	\N	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 22:16:18.123897	\N	\N	\N	\N	f
bd135d3d-4ee9-405a-9e1d-718866a21d4c	Nobis eum veniam odio aspernatur magnam mollitia ea sunt.	0	1	\N	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	c757d67e-b10d-49f4-b446-3010ae9f9591	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.124405	\N	\N	\N	\N	f
bfdd07d8-1b90-4b44-9bb9-cce021ac768d	Praesentium eos et sit a eum.	0	1	\N	\N	4929722e-df51-411e-8c00-59955f7d8fd8	682c7de5-825d-4037-b630-0662381923b7	\N	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 22:16:18.123765	\N	\N	\N	\N	f
c1a04c4d-eb1a-4a37-b296-03c017370585	Eius aut qui quia in.	0	1	\N	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.12471	\N	\N	\N	\N	f
c441bad4-ba6a-4552-a8d4-ecbccd32e6c9	Sed doloremque nam nisi quod et asperiores ut.	0	1	\N	\N	2e6b7127-5e54-43eb-a21f-64c57143824d	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	\N	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 22:16:18.12361	\N	\N	\N	\N	f
c8d6e098-d7e6-4d60-a5af-46956b02e231	Autem et quam molestiae consequuntur.	0	1	\N	\N	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	27b1736a-9ead-4a1a-9156-fa8922470eef	\N	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 22:16:18.12348	\N	\N	\N	\N	f
c997a394-ad61-44e6-a7c3-cdfedaadf4b8	Dolorem facere non dolor et consequuntur est excepturi dolor.	0	1	\N	\N	439c9800-35c9-48ee-8549-9c293a107743	5310a3e6-5a16-4090-a632-105cae7d42eb	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.1237	\N	\N	\N	\N	f
cb102234-0e8c-4790-b316-0cc544905e51	Aperiam aliquam qui asperiores vel quam consequuntur laborum.	0	1	\N	\N	fadd55dc-c457-41a6-9723-c259182f0035	2e68a27c-4347-4b83-91a5-a44e7c47473c	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:18.124144	\N	\N	\N	\N	f
cbbb454c-78a0-4ab6-a810-d57ce0613093	Velit perferendis cumque sit consequuntur incidunt.	0	1	\N	\N	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	\N	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 22:16:18.124446	\N	\N	\N	\N	f
cd5ec39d-5e90-4ce1-983d-6c232d2bd4c5	Id dolor officiis nesciunt id tempore ut ea sint et.	0	1	\N	\N	33725381-a183-49ef-b723-e70495ff6066	2edf5638-0175-4d58-81fc-92d37118727c	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:18.12404	\N	\N	\N	\N	f
d00c9da9-c035-486f-9242-82bae6228c93	Tenetur saepe maiores aliquam.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.12547	\N	\N	\N	\N	f
d1abb36b-cd35-4108-a7e0-c4aedc1187f0	Sed iste excepturi est assumenda voluptatibus.	0	1	\N	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.125186	\N	\N	\N	\N	f
d2a3ed64-6524-4963-bc2d-5cd85fdc586f	Ut ratione et veritatis sed soluta nihil unde.	0	1	\N	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	5310a3e6-5a16-4090-a632-105cae7d42eb	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:18.123456	\N	\N	\N	\N	f
d4bc8324-ab17-4ecd-9351-58dc4ed7a6f0	Quidem nulla sed vel libero eos consectetur.	0	1	\N	\N	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	\N	b7594574-0d60-4ffa-b14d-5917c719889b	2024-10-19 22:16:18.124842	\N	\N	\N	\N	f
d802602c-eb18-49e9-882f-02ee0347a3fb	Pariatur rerum repellendus qui non dignissimos earum a.	0	1	\N	\N	84609dec-b050-496e-81be-301a1334919a	ac641cb5-3883-4fbd-9783-9770175859f1	\N	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 22:16:18.124294	\N	\N	\N	\N	f
df0acfea-6b20-4559-8ec3-051f197b9a31	Rerum ipsam aut.	0	1	\N	\N	612e214e-4fe6-4b17-b9af-8b8b26bf180e	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	\N	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 22:16:18.125344	\N	\N	\N	\N	f
df8c330f-68cc-4c53-af44-cc76e632ee1f	Et suscipit dolores voluptatem animi culpa et debitis nisi.	0	1	\N	\N	d1372bba-be85-473c-8086-02a7c9890140	6b0a3307-50cd-4ab8-b239-52ec9227ff19	\N	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 22:16:18.123992	\N	\N	\N	\N	f
e7560122-8230-4d61-8eeb-6f46f0039978	Dolorem laboriosam laudantium quia facilis odio in impedit ipsam.	0	1	\N	\N	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	a5c74b1d-9279-441a-a283-8a9a67e6378c	\N	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 22:16:18.12379	\N	\N	\N	\N	f
ea33c66f-864b-47c2-9639-32bdd3ed1015	Deleniti aut sint.	0	1	\N	\N	6e132241-d674-4195-b8c5-b6b4d320e3ce	295c035f-0873-479b-b155-6746e039e598	\N	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 22:16:18.124535	\N	\N	\N	\N	f
eb47eea5-2f35-4a62-b12b-990a92d91836	In est ut sequi eveniet.	0	1	\N	\N	78532cb2-f350-4c98-bce2-e94afd8369c6	77bedc14-cd28-4fd7-8550-90cab9470ba4	\N	4bbe97ff-9028-4030-967e-34d7eae8f332	2024-10-19 22:16:18.123235	\N	\N	\N	\N	f
ed10e5e2-0d58-45f0-b665-26a54df771be	Temporibus fugit at maiores sunt dolores quod.	0	1	\N	\N	e095bbae-68d2-4077-9036-697c526736d4	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.125321	\N	\N	\N	\N	f
f84c141d-5769-4d8c-b6a7-31ebf642eb65	Cum magni dolore non quo est laboriosam facilis perspiciatis neque.	0	1	\N	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	27b1736a-9ead-4a1a-9156-fa8922470eef	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:18.12397	\N	\N	\N	\N	f
fa19a9a3-fcb1-46f1-9f6b-fd686324f1be	Nesciunt quasi laudantium.	0	1	\N	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.12523	\N	\N	\N	\N	f
fb6ec1b4-d2ef-46a3-8492-2c4b00392fa9	Est sequi velit veniam nam perspiciatis animi.	0	1	\N	\N	14baebc0-0189-423c-a14c-d62ffe981f63	27b1736a-9ead-4a1a-9156-fa8922470eef	\N	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:18.125015	\N	\N	\N	\N	f
fca984d4-d72f-465b-b563-d6964c8368e4	Minus harum non dicta.	0	1	\N	\N	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 22:16:18.123283	\N	\N	\N	\N	f
ffb05dcb-4ca2-4bf2-be2c-66d2226752d1	Minima omnis alias.	0	1	\N	\N	fe1e460d-16ac-4fcd-b512-2413b8cb3256	27b1736a-9ead-4a1a-9156-fa8922470eef	\N	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 22:16:18.123928	\N	\N	\N	\N	f
004e185b-bcc2-4544-88a6-201f9ea1e883	Et aut ut id sed nesciunt sed perspiciatis eos.	0	2	319fdcb4-3111-4fb1-843b-f09eca2550ca	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	\N	682004c3-1d38-4229-9731-07218d5f9aca	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:18.226466	\N	\N	\N	\N	f
031dd54e-833d-41ac-9d7d-03eeffb98d63	Eum in qui debitis aut rerum numquam provident pariatur dolorum.	0	2	aace8630-9cdb-40aa-a084-d6a0c5e0b8ed	\N	8b92673a-ba81-4629-aea9-41444a46a0dc	4dfeb3d3-2c9b-497e-8384-55e94215571d	bbaacc52-2375-4568-b6c0-ba65b96da990	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 22:16:18.226244	\N	\N	\N	\N	f
069e4aca-77ce-495a-b51d-3dd0c3124cb6	Nam velit adipisci veritatis temporibus quo.	0	0	16c628fd-72df-405c-b9f3-c67ccfe2ca36	\N	1f981aae-f40b-4dba-b383-8853d87b23c5	\N	\N	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 22:16:18.225696	\N	\N	\N	\N	f
0e9eb542-6945-465e-801e-cd7b4e4e0bd6	Quo voluptatum suscipit corrupti ratione consequatur dolor fugit.	0	0	ed193d73-004e-4abc-a93a-8d4b18aa82e0	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	\N	\N	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 22:16:18.225655	\N	\N	\N	\N	f
15328d73-217f-4ac9-a05a-24c48a987fe5	Tempora voluptatem saepe.	0	3	0cea666b-aa08-4b5a-9ad1-b89fcf9a7beb	\N	13ba9424-00b3-40a6-92c8-a9426207a2d9	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	b77b8654-b64f-44a3-8c8e-ef0786b9746b	d723eed5-78a1-4fab-9c9d-08efced4b861	2024-10-19 22:16:18.226488	\N	\N	\N	\N	f
181b34ea-9a8c-44e5-a6b5-dd5071ccf4f9	Nostrum rerum rerum.	0	1	2c444ce2-2b6b-41b6-8414-abce7486e726	\N	439c9800-35c9-48ee-8549-9c293a107743	\N	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.225591	\N	\N	\N	\N	f
1fc87fe6-c1e5-4611-88ca-58b05606824a	Fuga pariatur perferendis pariatur.	0	0	c529d666-1da3-4423-9c10-9f75b6299e82	\N	2e6b7127-5e54-43eb-a21f-64c57143824d	\N	\N	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 22:16:18.225205	\N	\N	\N	\N	f
2225ca3b-7628-422e-ba19-f702519183d8	Quia voluptas rerum perferendis accusamus et.	0	3	2ce94f37-21a2-4db3-a833-a545735f2869	\N	fe1e460d-16ac-4fcd-b512-2413b8cb3256	12b52fea-b990-404f-9f77-13d66ec80399	4bfc7c43-62e1-48a0-81dd-bfd19076854a	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 22:16:18.225572	\N	\N	\N	\N	f
274ebf3f-2d22-436c-b8c2-fe4539fcc606	Non animi a.	0	1	a68a3d70-b3d8-4ed2-aaa3-4ff392b4e88f	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	cef12cfc-5e1d-42c4-b7a0-90b57b0e2a36	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:18.226324	\N	\N	\N	\N	f
2c6911d9-8348-4394-8a68-fed006c6cc5b	Alias ullam illum.	0	2	e7a606de-db6b-4af9-8469-cc5e6132c119	\N	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	\N	e4db778b-0479-4a75-a211-c7283b1dbe95	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 22:16:18.225823	\N	\N	\N	\N	f
31d16018-b4d3-4df2-930a-7fc29c433b58	Iure sit est dolorum enim sunt ut.	0	2	d00c9da9-c035-486f-9242-82bae6228c93	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	f6b0f401-8c6e-4d62-a809-15d6006ee100	bbe595b8-2667-48fc-a592-14011264e4ff	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.226403	\N	\N	\N	\N	f
41c8d2c3-2142-45db-bb49-5ef29bcbcb3a	Quisquam esse doloribus aut.	0	0	fa19a9a3-fcb1-46f1-9f6b-fd686324f1be	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.225987	\N	\N	\N	\N	f
4b3a1f3e-2fe2-49dd-b262-45972b6e716a	Sed eveniet dignissimos libero pariatur sapiente.	0	3	4e75e413-3eb6-49a6-9a22-2b2e017e3bc7	\N	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	68151439-6890-4954-9559-06b02c84acdd	b8ab9725-24aa-44aa-abeb-4bafcf981af6	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 22:16:18.225303	\N	\N	\N	\N	f
4c079bc7-6769-45c7-add8-058a52248790	Suscipit totam dolor rerum sit quo aut deserunt.	0	0	fc507390-8c6e-404f-a09f-7a76385f3a1a	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	\N	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.226225	\N	\N	\N	\N	f
4e5d6c54-5979-41e8-815a-529a9eca08fc	Vel omnis ullam voluptas quaerat voluptas.	0	1	c51dd2e7-091c-46e2-a2d5-571ecfa07202	\N	fadd55dc-c457-41a6-9723-c259182f0035	\N	\N	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 22:16:18.225551	\N	\N	\N	\N	f
50546806-f9ff-489c-8ec7-96c9cd0ae599	Nam et voluptates error temporibus fugit assumenda qui nihil et.	0	2	4a17f2e2-1b1b-4544-a30e-8b267c5adf94	\N	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	\N	c9f32ba7-0163-42d7-a6f9-3f3b5e509dbe	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 22:16:18.225904	\N	\N	\N	\N	f
62fb3520-3d1c-4927-ac39-4f475b3dd371	Enim numquam id veritatis et ut et quo maiores.	0	3	59ebf797-2b58-47d6-bbcf-a3b54a0c0d00	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	d3935e8e-8abe-46dc-878a-4434da7af9ec	ddb36e7b-7ce7-4e93-b57a-a950d42ce00d	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.226104	\N	\N	\N	\N	f
64800c7b-efb6-4a2d-b7b0-6bf2da2b3d75	Voluptas error debitis magnam impedit qui.	0	2	3473d1df-17bd-4908-82e8-e0038baaee5b	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	d3935e8e-8abe-46dc-878a-4434da7af9ec	c6e35122-88fb-4ac3-b014-eaea5fbd0670	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.226206	\N	\N	\N	\N	f
666ddf15-e254-407b-9c43-d9fbb7bb6bec	Voluptatem voluptatem repellendus atque impedit asperiores corporis dolores.	0	0	45adf061-7651-48bc-961f-ad19ad28a780	\N	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 22:16:18.225756	\N	\N	\N	\N	f
6a02710f-e06e-4fa1-a7a6-fe6aded2d171	Aut necessitatibus qui.	0	0	59ebf797-2b58-47d6-bbcf-a3b54a0c0d00	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	d3935e8e-8abe-46dc-878a-4434da7af9ec	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.225862	\N	\N	\N	\N	f
6b237ecf-98b1-4a85-8c25-bebd529f4132	Consectetur repudiandae voluptatem non tenetur facere dolore ipsa fugiat quae.	0	0	4a17f2e2-1b1b-4544-a30e-8b267c5adf94	\N	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	\N	\N	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 22:16:18.225636	\N	\N	\N	\N	f
6cac249b-b3dc-4663-8a17-11aeffae751b	Quo officiis vero voluptas omnis repellat dolores nam sit omnis.	0	0	b5e780f4-f417-4e96-b35d-36cc34ba64ac	\N	eb1b0535-b7f3-430e-b91c-c1feea653f5f	\N	\N	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 22:16:18.226365	\N	\N	\N	\N	f
6d7c64c0-2b62-4a0d-86e6-9f7f24175a48	Quas ut ducimus ex molestiae veniam esse alias.	0	0	823df16c-8d73-4019-a23c-1bc3460d5738	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	\N	\N	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.225452	\N	\N	\N	\N	f
6e078dea-a423-4731-bc6c-96c1ddda7f57	Harum perspiciatis vero quo officiis nihil porro ullam minima dignissimos.	0	1	e2912da0-31ba-4a3a-8d12-a6b192e32ddf	\N	d1372bba-be85-473c-8086-02a7c9890140	\N	\N	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 22:16:18.226026	\N	\N	\N	\N	f
727804d6-e384-4191-a11e-b9e8b5f02804	Voluptate dolores vel id vero deserunt aut tempore.	0	0	320a585d-d980-4061-96c8-6f6c18370e84	\N	33725381-a183-49ef-b723-e70495ff6066	\N	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:18.22535	\N	\N	\N	\N	f
742c8286-463a-4f39-bf73-cbd9f1a61c2d	Fuga aut autem dolores mollitia qui consequatur iste dolore.	0	3	902439f5-1aa6-4778-ac4c-980efad02b9a	\N	00c05513-4129-4aa6-b79e-983ff13574fc	\N	c3ef21a7-4def-4f29-b1d9-e99378a59ea1	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:18.225798	\N	\N	\N	\N	f
75a6d518-0954-4bea-a94d-a954df376051	Et quas architecto harum labore distinctio exercitationem aut hic.	0	1	8c9f93f1-28e6-42e6-a0e9-082f4569bb9f	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	\N	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:18.226425	\N	\N	\N	\N	f
7d86ebd1-8840-4c66-ba10-df0ea69e1b89	Pariatur consequatur molestiae doloribus excepturi nostrum enim eligendi quia quia.	0	3	fc507390-8c6e-404f-a09f-7a76385f3a1a	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	c13d9508-227c-4909-888a-3d00a8acd407	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.225842	\N	\N	\N	\N	f
7e20e5c4-f3af-481b-852b-fd4fd9a86d65	Optio id labore qui ea aut dolore ipsam quis reprehenderit.	0	1	319fdcb4-3111-4fb1-843b-f09eca2550ca	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	\N	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 22:16:18.225513	\N	\N	\N	\N	f
7fee71ac-4b42-4c8e-bd78-bed51c0b4d33	Recusandae aut eius cum autem rerum totam ut aperiam earum.	0	0	ee1015a1-6246-40be-8820-52abbd0419dc	\N	978e2b3f-9c26-41f0-b3c4-cba2e492767f	\N	\N	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 22:16:18.226185	\N	\N	\N	\N	f
80dc7251-cba9-4d52-a833-d7ec7661dac4	Aut unde quidem corrupti commodi provident.	0	3	457a7fb5-e274-4eb6-acd9-0fadd53440fa	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	3091366c-bba5-4ab4-9565-558e8ba87cda	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:18.225532	\N	\N	\N	\N	f
866dcaa6-4a13-4559-8040-3890f95907f7	Maxime ipsa incidunt sapiente asperiores facere quaerat aperiam eos.	0	2	188a302b-92cc-494d-b8f8-b0bbc9873ea0	\N	14baebc0-0189-423c-a14c-d62ffe981f63	82e8961c-1ae8-4912-b4dc-51173b3fdfe6	0825a56f-8082-4c8a-ba8b-7388a263a95c	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:18.226265	\N	\N	\N	\N	f
867df512-86d4-43bf-ace8-bf15c2c01844	Officiis molestias repellat aut sit mollitia.	0	0	8c9f93f1-28e6-42e6-a0e9-082f4569bb9f	\N	705391da-77b5-4f08-b176-301a5f1bbc0d	\N	\N	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 22:16:18.226162	\N	\N	\N	\N	f
89ebb26a-beb1-4076-a285-229c00c8bbab	Ab eligendi facere fuga consequuntur recusandae consequatur ut.	0	0	c5074b4e-1119-402b-87cf-2e65df5b77dc	\N	9f64a38d-8cdd-4a21-a529-9747a9331998	\N	\N	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 22:16:18.226005	\N	\N	\N	\N	f
89efeac7-e7ae-4777-8408-b447a658581f	Magnam magnam illum officiis quia sit dolores.	0	1	bd135d3d-4ee9-405a-9e1d-718866a21d4c	\N	275ddc93-92b8-419a-ab96-baeb34d89c19	c757d67e-b10d-49f4-b446-3010ae9f9591	\N	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 22:16:18.225284	\N	\N	\N	\N	f
8b38a9fc-cc2a-491f-926c-adcb0dee4ea3	Cum voluptas dolorem incidunt deserunt sint ut eum atque.	0	2	ea33c66f-864b-47c2-9639-32bdd3ed1015	\N	6e132241-d674-4195-b8c5-b6b4d320e3ce	295c035f-0873-479b-b155-6746e039e598	ec808399-8df2-4489-8d88-8ebb5f06579b	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 22:16:18.22539	\N	\N	\N	\N	f
93bade5d-c27d-4561-850c-b2f508dc57c6	Fuga voluptatem quia culpa consequatur et.	0	2	995bf197-b6e7-4f5f-8d85-369bac021073	\N	50088da9-86e5-4781-be1e-f8b04a2554d0	5310a3e6-5a16-4090-a632-105cae7d42eb	6c772c1a-1541-4fa5-9a54-fda4310cf3b0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:18.225428	\N	\N	\N	\N	f
982a1e02-e133-4824-b682-2e9acb708e32	Saepe vel qui illum numquam fugit voluptatem aliquid veritatis nemo.	0	3	61a079c7-2810-4d9c-885a-4ec78635af08	\N	3652e96a-9dc0-4f12-817c-1ca7f05807e6	\N	2eac0593-97bb-43f5-9319-cce0477d855b	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 22:16:18.226045	\N	\N	\N	\N	f
9a4e73ac-4caf-4eb7-b5fc-294bafe000ca	Alias officia sapiente architecto voluptatem sequi dolores.	0	3	3473d1df-17bd-4908-82e8-e0038baaee5b	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	d3935e8e-8abe-46dc-878a-4434da7af9ec	b56fb161-b6d5-44bf-9c11-e7d0e9e285d0	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.225409	\N	\N	\N	\N	f
a0be5d50-fd44-4874-bc59-44a33da62b69	Tenetur sed vero accusantium porro rerum.	0	1	050e0d13-756f-4229-8be7-4e578f8b6017	\N	4929722e-df51-411e-8c00-59955f7d8fd8	682c7de5-825d-4037-b630-0662381923b7	\N	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 22:16:18.225946	\N	\N	\N	\N	f
a4af18d9-64a6-41ba-a59f-465bff132d78	Et veritatis corporis est et sed enim.	0	2	568e9391-1e65-4d4d-ab56-ae3355793bcb	\N	f18bc355-4a5c-4012-89a6-0044e4bfe36f	71691b66-37b0-4a42-95e2-f6d2c14a7d75	3839b2db-4d55-4c13-aff4-6b3f33c760b2	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.226123	\N	\N	\N	\N	f
a687cdc5-3e93-43e7-8c8e-2349907d9f0a	Provident architecto doloremque cupiditate qui eligendi.	0	0	461a8c38-7539-4e60-ac19-23bb39110b86	\N	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2edf5638-0175-4d58-81fc-92d37118727c	\N	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 22:16:18.226447	\N	\N	\N	\N	f
ada5a076-18b3-4a8a-8a78-c3638659a62d	Voluptas facilis repellat reiciendis exercitationem.	0	3	39c2d1ab-e652-408c-bf92-c296bcb881e8	\N	14baebc0-0189-423c-a14c-d62ffe981f63	\N	ef1ff5b1-9058-4991-a869-72479c131fb7	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 22:16:18.225776	\N	\N	\N	\N	f
ae341c84-73c0-4578-82b9-c92650a2a970	Animi est delectus.	0	0	85701256-203b-4049-8dcb-03bd45a50d9c	\N	143437a3-503e-4e95-911d-4c6571ddea8e	\N	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 22:16:18.225471	\N	\N	\N	\N	f
af71eb0e-00b2-4b94-b3ce-f977873d9c5b	Architecto iste est atque eos qui magnam nobis molestiae nisi.	0	0	6649c0f6-58e9-4565-b55c-9ce264140d73	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:18.226285	\N	\N	\N	\N	f
b7fab6f7-79f6-4438-9280-58b6c9c38eed	Quos non illo est tenetur non consequatur voluptas.	0	1	b6aed98a-9fcb-4222-af33-35acffae049a	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	\N	\N	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 22:16:18.225965	\N	\N	\N	\N	f
b875b24a-1594-4c1d-a2c7-b5c3ec56ae70	Dolorem illum molestiae dolore in pariatur.	0	2	01a7b9f6-5c0e-4978-98e8-9799436f2219	\N	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	a034356e-290e-4bf8-b6eb-fd5e8f6d3aba	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:18.22533	\N	\N	\N	\N	f
ba6e7b86-6a60-4c97-acab-8160a0e69064	Accusamus rerum magni earum qui natus nobis.	0	1	f393b944-bc5c-4f3f-b3fd-355170e17a06	\N	72843603-7dc4-4405-92fa-9a7289ac9b66	\N	\N	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 22:16:18.22549	\N	\N	\N	\N	f
bd0d57de-f935-4a9e-8ebb-7192d6428c02	Ut quo autem est fuga repellendus quos quia amet quae.	0	2	4303357a-c0c2-46c1-805d-5c8c3d4102fa	\N	ed964db3-afac-426e-8988-c2ed54a89510	\N	d3750a28-fee9-4a16-869c-57f1029c94f7	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 22:16:18.226343	\N	\N	\N	\N	f
c1ba8e37-d159-49a1-87b9-6c3b7aa292c8	Qui ut ea corporis nulla.	0	2	f004d9c3-5999-4725-8d97-dd7953416005	\N	30d72372-2aee-46cd-ab7f-56dcaefe600c	\N	8c51ef0b-d01a-44ba-98d5-8dc046e3d6cb	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.226303	\N	\N	\N	\N	f
c34ad7d4-f379-475e-88ab-2d9400915f92	Et mollitia consequatur delectus rerum velit accusantium voluptas.	0	3	a229dba8-e286-4046-97bc-0281dad42e56	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	a7c48de9-4698-47cf-ad56-cbaaede24885	57db9074-fed8-4fc3-8698-4650d8688b37	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.225737	\N	\N	\N	\N	f
c93787c3-586f-4a3e-8df5-21763d41de7e	Est incidunt sint.	0	3	823df16c-8d73-4019-a23c-1bc3460d5738	\N	9ca9bcee-c97f-4778-83f4-57fff49759d1	\N	286ba88a-d83e-457f-855b-5e316443980c	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.225369	\N	\N	\N	\N	f
caceb32e-0726-42b6-8c5f-534e255f3408	Eos ducimus sunt et quia dolorem rerum et.	0	3	e7a606de-db6b-4af9-8469-cc5e6132c119	\N	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	\N	efbbec48-d9ea-493f-a9d6-b7d379d7b874	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 22:16:18.225927	\N	\N	\N	\N	f
ce0c7396-1058-422a-8d3a-eeadc556234e	Et veritatis quia animi praesentium quo esse.	0	1	ed10e5e2-0d58-45f0-b665-26a54df771be	\N	e095bbae-68d2-4077-9036-697c526736d4	f6b0f401-8c6e-4d62-a809-15d6006ee100	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.225677	\N	\N	\N	\N	f
d1005624-0ed7-4e6c-b439-c69803eaff1a	Sed fuga ipsa pariatur laboriosam sunt id voluptatum nam.	0	1	6649c0f6-58e9-4565-b55c-9ce264140d73	\N	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	\N	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 22:16:18.226384	\N	\N	\N	\N	f
d30d525b-ffe4-4f38-9101-73d72e204fec	Est omnis soluta ut odio similique recusandae accusamus rerum.	0	0	41b6bfe1-00ce-49d7-a5cd-606aaf2ce118	\N	00c05513-4129-4aa6-b79e-983ff13574fc	ac641cb5-3883-4fbd-9783-9770175859f1	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 22:16:18.226144	\N	\N	\N	\N	f
d3ee38a3-daa9-4a97-9459-64afd4b188d2	Ut repellendus voluptas ab repellat velit.	0	3	12e7be20-4901-4b1c-b5e8-9ec296a51652	\N	959b7d55-62bf-42c0-a313-33054551abb5	\N	741a2c19-1d9e-44bf-8b12-0b6634b7c840	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:18.226085	\N	\N	\N	\N	f
e986b146-9310-4e6c-b861-08f6d473883f	Numquam dolor dolorem minus quis ut quis voluptatem.	0	0	b62ec2e6-6c4f-4088-939d-5b23fa1e1347	\N	53453386-8816-485f-9a08-22c07cf29d22	2edf5638-0175-4d58-81fc-92d37118727c	\N	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 22:16:18.225614	\N	\N	\N	\N	f
eec07239-3c5e-4df3-af8c-5144cbff339a	Et eaque ea at sit.	0	1	238d8aca-b2ef-437c-a8af-9befaa9aab72	\N	f015b253-2d06-44b2-8f52-1ae49c1a241c	\N	\N	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 22:16:18.225881	\N	\N	\N	\N	f
fcc9ab35-8f4f-41ed-885c-ec0924ec54d7	Maiores ex voluptatibus voluptas ullam dolores doloribus omnis debitis ex.	0	3	66773bd4-a0bb-43ba-995f-df0e3af42f43	\N	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	000a98cc-5e30-4beb-8fe1-228df3f2cb4e	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 22:16:18.226064	\N	\N	\N	\N	f
fe0b6fa7-a6cb-4536-98b2-7d6d514c8f50	Porro quam et maxime deleniti vel ut necessitatibus unde autem.	0	3	e08ace5f-c6f8-452f-9625-c78299b32857	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	69916948-3814-40d5-80d3-2cb0e9b4703f	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:18.225714	\N	\N	\N	\N	f
0496365b-89b0-41de-a778-2b3dc4740d50	Sunt porro optio accusantium rerum esse.	0	0	\N	2111cae2-60b0-42d8-9e6f-f9d39abef4fd	7b42cb26-668a-4037-8ffc-68058704a460	\N	\N	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 22:16:18.275443	\N	\N	\N	\N	f
05b4cdc0-dd43-4d76-b0d1-0af67506657e	Esse qui quas rem autem sed.	0	0	\N	df0acfea-6b20-4559-8ec3-051f197b9a31	83c97377-4790-4e12-9b61-c0456fe642b2	\N	\N	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 22:16:18.275109	\N	\N	\N	\N	f
095c919c-9c6e-4611-b34b-b1bf1bd929e9	Sunt totam labore rerum voluptates quidem veniam.	0	0	\N	df8c330f-68cc-4c53-af44-cc76e632ee1f	f18bc355-4a5c-4012-89a6-0044e4bfe36f	\N	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.275037	\N	\N	\N	\N	f
0b66faeb-ae55-4dce-a53f-57ddce4a6b84	Quia non quidem non vitae perferendis.	0	0	\N	7e20e5c4-f3af-481b-852b-fd4fd9a86d65	6700632c-6c3b-4d7e-81dd-8b2151b60502	\N	\N	6d48e156-8327-48d6-91d9-61ce20e3125b	2024-10-19 22:16:18.275394	\N	\N	\N	\N	f
1cc00b44-65b1-4887-a9d3-76d1fa7e02ad	Et non suscipit aut molestiae beatae laboriosam.	0	0	\N	59ebf797-2b58-47d6-bbcf-a3b54a0c0d00	9ca9bcee-c97f-4778-83f4-57fff49759d1	\N	\N	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 22:16:18.274235	\N	\N	\N	\N	f
1d6683f7-acb1-42c5-ac49-2434be0d880b	Quo sapiente numquam.	0	0	\N	e08ace5f-c6f8-452f-9625-c78299b32857	439c9800-35c9-48ee-8549-9c293a107743	\N	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.274953	\N	\N	\N	\N	f
210f8df3-5d95-4ff1-b40f-d5d8a05c16be	Sed modi enim voluptatem cupiditate qui eaque.	0	0	\N	9dac6413-22d2-4a94-a37d-d58848d34e6c	84609dec-b050-496e-81be-301a1334919a	\N	\N	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 22:16:18.275566	\N	\N	\N	\N	f
2274ea1d-deb9-4fad-91e9-638d10ec3fb6	Qui sint voluptatem expedita est.	0	0	\N	ea33c66f-864b-47c2-9639-32bdd3ed1015	2fa772f8-0fa4-472b-a154-14cf794d50e2	\N	\N	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 22:16:18.275349	\N	\N	\N	\N	f
27e7906b-c59e-464f-968d-2b4ceab3badf	Excepturi et laborum quia quo harum quod odio.	0	0	\N	319fdcb4-3111-4fb1-843b-f09eca2550ca	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	\N	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:18.274324	\N	\N	\N	\N	f
2e6e096f-f63b-47e7-8393-72236adfbbcf	Aperiam architecto dolore dolorem et officiis non quas.	0	0	\N	448dda8e-53c0-4898-9d41-790dcfec13f1	bb05cc9c-87a1-4d43-b253-d172e2117ff2	\N	\N	694020bc-a98b-4a12-93da-c9331c68619a	2024-10-19 22:16:18.274497	\N	\N	\N	\N	f
30d9a802-77dc-4d81-96a0-381c1d0ccc9d	Neque in ut.	0	0	\N	8824c253-9f54-4774-a138-ca72078a4892	45370c44-1d4d-4834-8cd5-3551b5d30199	\N	\N	d34efe03-6baf-42df-8e7b-0418ac94c7f8	2024-10-19 22:16:18.275369	\N	\N	\N	\N	f
3348748b-a0b8-40bd-9a27-8a393aed83b7	Perferendis in ut saepe similique nisi.	0	0	\N	ada5a076-18b3-4a8a-8a78-c3638659a62d	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	\N	\N	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 22:16:18.27491	\N	\N	\N	\N	f
3519b7d7-092c-4693-b2b7-71010b00fc56	Quia quis blanditiis alias optio sit.	0	0	\N	c46c8edc-347c-4d66-b9c5-72db6ef5dda5	c6d25490-d32a-410d-be77-5370cc1482a2	\N	\N	d776b5c6-c7ca-4d5a-9fd3-6d2828447425	2024-10-19 22:16:18.274214	\N	\N	\N	\N	f
396ffb1a-1e41-4056-ac59-d1d4ba7c558e	Nam magnam odio minus tempore ut ipsa dolores neque veritatis.	0	0	\N	869a61b9-eae9-4a1a-ad8d-14abf04f2797	959b7d55-62bf-42c0-a313-33054551abb5	\N	\N	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:18.27489	\N	\N	\N	\N	f
3ab1c5c4-1c1d-4c77-bc0f-10d83278ed91	Earum sint enim facilis voluptas aut saepe voluptatem rerum temporibus.	0	0	\N	daab4c04-b414-46d6-a837-4eed074ff221	e095bbae-68d2-4077-9036-697c526736d4	\N	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.275245	\N	\N	\N	\N	f
3c58deb5-d139-441c-9f37-7ffa064addb9	Harum molestiae odio.	0	0	\N	ed10e5e2-0d58-45f0-b665-26a54df771be	b55f5bbd-4b95-448a-b38b-a1429002854b	\N	\N	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 22:16:18.274364	\N	\N	\N	\N	f
3cc537b3-d931-4fff-98d9-a2e002e25ad0	Earum qui quis dolorum rerum.	0	0	\N	2ce94f37-21a2-4db3-a833-a545735f2869	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	\N	\N	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 22:16:18.274994	\N	\N	\N	\N	f
426bca18-f6c4-4026-b020-57e9ed5c6ae0	Perspiciatis sint illo numquam sequi placeat unde.	0	0	\N	2ce94f37-21a2-4db3-a833-a545735f2869	0b996fe8-4582-412b-adfb-6fa402c25bf4	\N	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 22:16:18.27565	\N	\N	\N	\N	f
4425a0b2-7b9a-414e-83ef-3ce81cc9800b	Laborum sint iste optio est nemo et id.	0	0	\N	727804d6-e384-4191-a11e-b9e8b5f02804	e095bbae-68d2-4077-9036-697c526736d4	\N	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.274826	\N	\N	\N	\N	f
44fca936-1177-43f1-a0bf-d1651a2d7e83	Illo expedita velit odit quod optio similique et.	0	0	\N	ad0dc67e-662a-4188-bf86-141406219972	439c9800-35c9-48ee-8549-9c293a107743	\N	\N	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 22:16:18.274132	\N	\N	\N	\N	f
53e947fd-a6c3-423c-9083-d461661fafaa	Et enim molestias enim quas nihil voluptates.	0	0	\N	ada5a076-18b3-4a8a-8a78-c3638659a62d	f18bc355-4a5c-4012-89a6-0044e4bfe36f	\N	\N	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 22:16:18.275463	\N	\N	\N	\N	f
5f31ff8e-b204-4ec7-b2a3-63c9f920b9f2	Sit ratione et nisi ratione amet reprehenderit omnis mollitia et.	0	0	\N	3d716e90-afdb-4d06-9ba5-c90cebbd1204	b6d54f8d-b08c-4f88-9db9-00008875256f	\N	\N	120acdc1-8799-412b-8fc8-67addf841f25	2024-10-19 22:16:18.275609	\N	\N	\N	\N	f
62487b05-8e6c-4315-8fe4-0a5592c04ae0	Earum sit perspiciatis quia molestias accusamus qui molestiae magnam.	0	0	\N	eb47eea5-2f35-4a62-b12b-990a92d91836	e095bbae-68d2-4077-9036-697c526736d4	\N	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.274407	\N	\N	\N	\N	f
65406abe-90d9-4ed3-aaee-1aeebc998ab4	Quisquam fugiat impedit officia distinctio recusandae.	0	0	\N	a68a3d70-b3d8-4ed2-aaa3-4ff392b4e88f	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	\N	\N	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 22:16:18.274645	\N	\N	\N	\N	f
673700bf-6c88-446d-9973-27993325a29b	Et debitis ipsa omnis ab doloribus.	0	0	\N	a881878c-730e-4f8c-9af3-9eb2b6880231	50088da9-86e5-4781-be1e-f8b04a2554d0	\N	\N	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 22:16:18.275524	\N	\N	\N	\N	f
69f35f1e-a4ba-4b78-b5a7-ef51c6f9cb18	Sed necessitatibus hic et error doloremque.	0	0	\N	50546806-f9ff-489c-8ec7-96c9cd0ae599	2e6b7127-5e54-43eb-a21f-64c57143824d	\N	\N	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 22:16:18.274727	\N	\N	\N	\N	f
6f9bbb29-6f1a-44fd-a0cd-4d8cb19ff345	Aut temporibus doloribus magnam aliquid eos consequuntur dolores rerum inventore.	0	0	\N	866dcaa6-4a13-4559-8040-3890f95907f7	eba19f8f-0936-45eb-88bc-9c83772a1d93	\N	\N	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 22:16:18.275089	\N	\N	\N	\N	f
7317f788-1802-4b52-8aaf-26081abd2b10	Quam molestiae perferendis itaque laudantium quasi consequuntur.	0	0	\N	727804d6-e384-4191-a11e-b9e8b5f02804	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	\N	\N	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 22:16:18.275151	\N	\N	\N	\N	f
75b9622a-4032-4105-b6c3-31e8dbe91126	Maiores ullam velit et et id.	0	0	\N	53de4d12-ab1c-42a0-b1fd-06228672fc3f	b3243d6a-7be2-4c83-8a89-dfd4a1976095	\N	\N	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 22:16:18.275202	\N	\N	\N	\N	f
7c76779a-3b80-4086-8856-a2954d405acb	Eum illum laboriosam id in omnis voluptas est dolorum voluptas.	0	0	\N	c34ad7d4-f379-475e-88ab-2d9400915f92	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	\N	\N	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 22:16:18.275292	\N	\N	\N	\N	f
7e3e036a-9af8-45ce-83a2-b681eb5ecfb2	Sunt similique qui.	0	0	\N	3473d1df-17bd-4908-82e8-e0038baaee5b	e00c9a01-ea24-48db-ac41-4d39c79f9321	\N	\N	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 22:16:18.274665	\N	\N	\N	\N	f
88d91836-9fdf-49e2-a076-e2861ca1e3e0	Quam nulla quia asperiores accusamus quisquam cum consequatur.	0	0	\N	7b0c1c45-6de6-4c60-b2df-b2488dae411e	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	\N	\N	15d219ed-b4eb-46de-9f55-741dd7dcec62	2024-10-19 22:16:18.274748	\N	\N	\N	\N	f
89f75794-f9e8-4da8-b8ee-3cfd2959fb66	Aspernatur nobis delectus quia eos suscipit maiores.	0	0	\N	cd32c9c6-8f6d-4dd1-8f04-b0999044b264	fa846317-fe54-4f52-b8d6-6a618387a5da	\N	\N	b56dfb50-cf66-498e-80b8-61876a9c4d92	2024-10-19 22:16:18.275182	\N	\N	\N	\N	f
8b45f5f3-3d35-4e79-b47b-7ec1007ce8ec	Sint corporis aspernatur.	0	0	\N	96a9e80b-01fa-4df2-8f0e-a1d4656f69b3	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	\N	\N	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 22:16:18.275505	\N	\N	\N	\N	f
90bb6a5b-fe64-41cd-96f0-2b579423fa16	Et ea numquam quam.	0	0	\N	8be9cc31-3a14-44ad-b689-9d7c01c79fcc	e095bbae-68d2-4077-9036-697c526736d4	\N	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 22:16:18.274387	\N	\N	\N	\N	f
9a3f2df8-8c74-420c-a3ba-b67f0b6ff0e5	Laboriosam natus tenetur in sit.	0	0	\N	75a6d518-0954-4bea-a94d-a954df376051	83c97377-4790-4e12-9b61-c0456fe642b2	\N	\N	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 22:16:18.274301	\N	\N	\N	\N	f
9ae1428f-9549-4ef3-b911-da27a90b1499	Ea commodi temporibus ut et consequatur itaque exercitationem sit et.	0	0	\N	fbf6561e-a09c-4608-a3fa-873a2026a921	fe1e460d-16ac-4fcd-b512-2413b8cb3256	\N	\N	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 22:16:18.275419	\N	\N	\N	\N	f
a22d6ae1-0f2f-4811-89f6-3955cf6690f8	Fugit ut et eligendi.	0	0	\N	7d86ebd1-8840-4c66-ba10-df0ea69e1b89	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	\N	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:18.274796	\N	\N	\N	\N	f
a5fe1bfc-f52b-49e2-af95-abe35c75c0d4	Dolor illum vel corrupti ut.	0	0	\N	6a02710f-e06e-4fa1-a7a6-fe6aded2d171	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	\N	\N	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 22:16:18.275588	\N	\N	\N	\N	f
a8e1a637-887e-41a2-a8d6-cb5a2f34ea90	Deserunt non at rem.	0	0	\N	cd5ec39d-5e90-4ce1-983d-6c232d2bd4c5	1f981aae-f40b-4dba-b383-8853d87b23c5	\N	\N	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 22:16:18.274582	\N	\N	\N	\N	f
aa4b87f6-f5ee-48b2-8be9-c04f94f2dc05	Itaque molestiae vitae nulla dicta impedit voluptatibus delectus quo.	0	0	\N	a687cdc5-3e93-43e7-8c8e-2349907d9f0a	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	\N	\N	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 22:16:18.274452	\N	\N	\N	\N	f
aaf39ee7-88b6-4b34-b7a8-2dc32b3fcca2	Ratione rerum consectetur eos aut dicta optio placeat autem.	0	0	\N	c8d6e098-d7e6-4d60-a5af-46956b02e231	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	\N	\N	6af636c4-96e4-4f9e-96a0-794dc6541dc3	2024-10-19 22:16:18.275224	\N	\N	\N	\N	f
b2c8cf9a-38a5-4b00-89d3-1b124fb8e7ab	Voluptas ut fuga suscipit.	0	0	\N	7a93e6e0-7225-4bca-9865-96108b806aed	ae5d22bf-3855-460b-a502-9747f35d6a34	\N	\N	f1423b81-e629-47f3-96fd-6fc76e094f58	2024-10-19 22:16:18.275628	\N	\N	\N	\N	f
b5284350-024e-47d7-b6da-becb1e0f7c27	Aliquam voluptatem inventore atque aut tenetur amet architecto.	0	0	\N	b6aed98a-9fcb-4222-af33-35acffae049a	1f981aae-f40b-4dba-b383-8853d87b23c5	\N	\N	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 22:16:18.274974	\N	\N	\N	\N	f
b54fd375-eb95-412e-9e0f-247e6151b077	Voluptatibus incidunt deserunt nobis cum quidem est tempora sed.	0	0	\N	bd135d3d-4ee9-405a-9e1d-718866a21d4c	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	\N	\N	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 22:16:18.274281	\N	\N	\N	\N	f
ba4ffe60-7239-482f-8c73-433379791032	Accusantium sit deleniti.	0	0	\N	275cc92f-a3c8-4c27-b725-86084476f9d0	30d72372-2aee-46cd-ab7f-56dcaefe600c	\N	\N	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 22:16:18.27452	\N	\N	\N	\N	f
bcf5bcbc-7bbd-4904-b14f-7eb3e5954de1	Autem laboriosam et id reiciendis ut nam quidem veritatis voluptatum.	0	0	\N	cd5ec39d-5e90-4ce1-983d-6c232d2bd4c5	143437a3-503e-4e95-911d-4c6571ddea8e	\N	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 22:16:18.275547	\N	\N	\N	\N	f
c2753236-1fca-4934-af38-1db51bbbcd14	Et quis error eum officia autem.	0	0	\N	6b237ecf-98b1-4a85-8c25-bebd529f4132	1faf9d72-1396-4e99-935d-547b226327c7	\N	\N	3054da29-a2e4-41b0-b7ac-9f3f4769e461	2024-10-19 22:16:18.275017	\N	\N	\N	\N	f
c6aaddb8-b832-4a5a-853e-59443b7e1879	Labore omnis et aspernatur quia quis incidunt.	0	0	\N	eb26d5e7-be4d-4d5b-b43c-189a9295f9c2	33725381-a183-49ef-b723-e70495ff6066	\N	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 22:16:18.274776	\N	\N	\N	\N	f
cb720883-83ac-41fc-a508-4655d8d15a50	Similique est numquam modi quibusdam.	0	0	\N	01a7b9f6-5c0e-4978-98e8-9799436f2219	22e64c46-97c3-40a7-a4aa-4b11eb838446	\N	\N	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 22:16:18.274602	\N	\N	\N	\N	f
cbbeac70-6943-40a2-9bff-5b89e3694705	Velit ducimus iste dicta nostrum maiores quaerat omnis.	0	0	\N	11661088-169b-469c-a5f7-d262d850f411	2fa772f8-0fa4-472b-a154-14cf794d50e2	\N	\N	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 22:16:18.274477	\N	\N	\N	\N	f
cce4a61e-3ac7-474a-b1bd-84a4e65a44fc	Eos dicta at et aut deserunt voluptas.	0	0	\N	057fdc9c-9585-4938-9591-b96847735e02	84609dec-b050-496e-81be-301a1334919a	\N	\N	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 22:16:18.274847	\N	\N	\N	\N	f
d12e430b-9e3c-450a-81d0-02109dcb8d78	Reiciendis aut sapiente consectetur.	0	0	\N	35aab92b-6e5c-48ef-86b4-1f0e1254f75a	f015b253-2d06-44b2-8f52-1ae49c1a241c	\N	\N	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 22:16:18.27454	\N	\N	\N	\N	f
d131b1a6-54df-4589-afcf-c605b2088a21	Maxime soluta molestiae voluptatem animi omnis.	0	0	\N	74ec5be2-f419-40c4-8981-58aa802f773b	b0d1d45b-c71b-4578-8ac0-01c30b49131b	\N	\N	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 22:16:18.275066	\N	\N	\N	\N	f
d3dd335f-564e-4f70-87af-661231c4c4ca	Quae iste quasi accusantium magnam porro dolor soluta.	0	0	\N	73d65748-80de-4816-a6e9-c2176cb80e97	45370c44-1d4d-4834-8cd5-3551b5d30199	\N	\N	d34efe03-6baf-42df-8e7b-0418ac94c7f8	2024-10-19 22:16:18.275129	\N	\N	\N	\N	f
d843cc47-3cb5-473f-89e2-0afc59b572ef	Exercitationem qui quisquam temporibus velit numquam officiis.	0	0	\N	9a4e73ac-4caf-4eb7-b5fc-294bafe000ca	8b92673a-ba81-4629-aea9-41444a46a0dc	\N	\N	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 22:16:18.274625	\N	\N	\N	\N	f
d96dd834-5149-4fa0-a777-87a515dd4b37	Corrupti reprehenderit neque quae laudantium quia iusto.	0	0	\N	15328d73-217f-4ac9-a05a-24c48a987fe5	959b7d55-62bf-42c0-a313-33054551abb5	\N	\N	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:18.274867	\N	\N	\N	\N	f
dc993102-3e7a-449f-b23c-58cc618836a8	Quidem quae harum quia inventore sapiente ut totam sed.	0	0	\N	f004d9c3-5999-4725-8d97-dd7953416005	78532cb2-f350-4c98-bce2-e94afd8369c6	\N	\N	4bbe97ff-9028-4030-967e-34d7eae8f332	2024-10-19 22:16:18.275485	\N	\N	\N	\N	f
df86f8b4-681f-4c19-8d6e-a9378deee77c	Libero dolore non voluptates suscipit et.	0	0	\N	5f59de6e-8c5f-4654-986c-edd7ba6cc316	959b7d55-62bf-42c0-a313-33054551abb5	\N	\N	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 22:16:18.275269	\N	\N	\N	\N	f
e7cc24db-1114-4841-acbf-c7162cdb6ef1	Facilis perferendis expedita odio quis.	0	0	\N	53de4d12-ab1c-42a0-b1fd-06228672fc3f	09f405ed-f0c6-422c-847f-0e24f7c74aef	\N	\N	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 22:16:18.27426	\N	\N	\N	\N	f
ea2ea9c1-c166-4b98-85bf-fc799128c7a8	Nobis incidunt animi adipisci.	0	0	\N	03282ed9-1f38-446e-9111-8a2061652e74	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	\N	\N	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 22:16:18.27456	\N	\N	\N	\N	f
ed54c57d-aee0-4f4c-b9b6-460e6f7d6fe2	Impedit cum aut.	0	0	\N	48977d7c-9b80-4c61-8abc-f7559aa0a8f6	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	\N	\N	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 22:16:18.274427	\N	\N	\N	\N	f
ef3b1005-fa04-4f9b-9c1d-b81a343bef17	Dolorem aut ut blanditiis natus aut adipisci qui atque.	0	0	\N	4c079bc7-6769-45c7-add8-058a52248790	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	\N	\N	b6a3426d-d4da-49e2-b18e-eb40caad3700	2024-10-19 22:16:18.274687	\N	\N	\N	\N	f
efb13e7e-40e0-48a2-8255-ac007c00dc24	Quisquam distinctio sint.	0	0	\N	0a0594cf-c807-46a2-b2d5-9ed61693e70e	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	\N	\N	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 22:16:18.275312	\N	\N	\N	\N	f
f3038d4a-0208-4114-bb3d-bf47f3b9b6f0	Et similique quasi id ducimus ut vitae recusandae fuga consequatur.	0	0	\N	89efeac7-e7ae-4777-8408-b447a658581f	18e845d8-400b-4d12-a414-9cd440f92677	\N	\N	ecfbb998-043f-40d3-af8c-c0a0cf04f57b	2024-10-19 22:16:18.274707	\N	\N	\N	\N	f
fa71e15c-0f9d-4a89-9516-face4ca8c997	Molestias consequuntur perspiciatis vel illo aperiam ab sed dolorem ut.	0	0	\N	fb6ec1b4-d2ef-46a3-8492-2c4b00392fa9	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	\N	\N	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 22:16:18.27493	\N	\N	\N	\N	f
fe7e27fd-0717-4869-a9e6-41de004b0778	Repudiandae recusandae alias pariatur accusamus omnis suscipit suscipit aspernatur necessitatibus.	0	0	\N	fcc9ab35-8f4f-41ed-885c-ec0924ec54d7	9f64a38d-8cdd-4a21-a529-9747a9331998	\N	\N	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 22:16:18.274344	\N	\N	\N	\N	f
\.

--
-- TOC entry 2846 (class 2606 OID 16644)
-- Name: posts PK_posts; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.posts
    ADD CONSTRAINT "PK_posts" PRIMARY KEY (id);


--
-- TOC entry 2843 (class 1259 OID 16655)
-- Name: IX_posts_parent_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE INDEX "IX_posts_parent_id" ON public.posts USING btree (parent_id);


--
-- TOC entry 2844 (class 1259 OID 16656)
-- Name: IX_posts_presentation_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE INDEX "IX_posts_presentation_id" ON public.posts USING btree (presentation_id);


--
-- TOC entry 2847 (class 2606 OID 16645)
-- Name: posts FK_posts_posts_parent_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.posts
    ADD CONSTRAINT "FK_posts_posts_parent_id" FOREIGN KEY (parent_id) REFERENCES public.posts(id);


--
-- TOC entry 2848 (class 2606 OID 16650)
-- Name: posts FK_posts_posts_presentation_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.posts
    ADD CONSTRAINT "FK_posts_posts_presentation_id" FOREIGN KEY (presentation_id) REFERENCES public.posts(id) ON DELETE CASCADE;


-- Completed on 2024-10-19 15:58:51 UTC

--
-- PostgreSQL database dump complete
--

