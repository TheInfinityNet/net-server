--
-- PostgreSQL database dump
--

-- Dumped from database version 12.20 (Debian 12.20-1.pgdg110+1)
-- Dumped by pg_dump version 12.20

-- Started on 2024-10-19 16:00:14 UTC
\c relationship_service_db

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
-- TOC entry 2 (class 3079 OID 16415)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 2988 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 204 (class 1259 OID 16743)
-- Name: friendships; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.friendships (
    id uuid NOT NULL,
    status integer NOT NULL,
    sender_id uuid NOT NULL,
    receiver_id uuid NOT NULL,
    created_by uuid,
    created_at timestamp without time zone NOT NULL,
    updated_by uuid,
    updated_at timestamp without time zone,
    deleted_by uuid,
    deleted_at timestamp without time zone,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.friendships OWNER TO "infinitynetUser";

--
-- TOC entry 205 (class 1259 OID 16748)
-- Name: interactions; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.interactions (
    id uuid NOT NULL,
    type integer NOT NULL,
    user_profile_id uuid NOT NULL,
    relate_profile_id uuid NOT NULL,
    friendship_id uuid,
    created_by uuid,
    created_at timestamp without time zone NOT NULL,
    updated_by uuid,
    updated_at timestamp without time zone,
    deleted_by uuid,
    deleted_at timestamp without time zone,
    is_deleted boolean NOT NULL
);

ALTER TABLE public.interactions OWNER TO "infinitynetUser";

--
-- TOC entry 2981 (class 0 OID 16743)
-- Dependencies: 204
-- Data for Name: friendships; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.friendships (id, status, sender_id, receiver_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
071f2036-530e-449a-a26b-68187e220cd0	1	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	7374bc88-8afb-4236-9fa0-d75adad253a0	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 21:53:51.145621	\N	\N	\N	\N	f
08665f8a-8321-4706-af5c-268c191d05ab	1	09f405ed-f0c6-422c-847f-0e24f7c74aef	49fa1298-7d26-4de1-b197-3005c3d03c0e	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 21:53:51.145863	\N	\N	\N	\N	f
089453e3-a848-493e-a2ef-1d63b1327921	1	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	f015b253-2d06-44b2-8f52-1ae49c1a241c	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 21:53:51.145505	\N	\N	\N	\N	f
0c8d86e8-c35a-482e-b05e-f74a576f0a02	1	978e2b3f-9c26-41f0-b3c4-cba2e492767f	7b42cb26-668a-4037-8ffc-68058704a460	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:53:51.145978	\N	\N	\N	\N	f
11e5a7f4-8345-43e9-9f24-94472a99ec4c	1	74d9ea46-5729-454f-b994-8faee093ddef	d1372bba-be85-473c-8086-02a7c9890140	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 21:53:51.145815	\N	\N	\N	\N	f
16c85f0d-ffe9-4c77-9608-a9e651333eff	1	09f405ed-f0c6-422c-847f-0e24f7c74aef	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 21:53:51.145779	\N	\N	\N	\N	f
1f6a569c-bfa5-47d1-91bf-9a8d18aaf037	1	f015b253-2d06-44b2-8f52-1ae49c1a241c	2fa772f8-0fa4-472b-a154-14cf794d50e2	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 21:53:51.145643	\N	\N	\N	\N	f
20444228-c7c7-4518-9790-7ae0b7796885	1	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	3d8be820-f83f-4579-b8e2-a8c4b5465d69	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 21:53:51.146068	\N	\N	\N	\N	f
217cddb3-1657-4bd1-a7a5-e6a45de98621	1	b55f5bbd-4b95-448a-b38b-a1429002854b	eb1b0535-b7f3-430e-b91c-c1feea653f5f	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:53:51.145752	\N	\N	\N	\N	f
26a99127-3bd3-4a0c-885c-16ea295fa8ba	1	eb1b0535-b7f3-430e-b91c-c1feea653f5f	9612f20e-6fce-4190-bc29-b31d7d3d9188	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 21:53:51.145888	\N	\N	\N	\N	f
28ccbb65-4412-4d73-9ddb-027e46e56748	1	e095bbae-68d2-4077-9036-697c526736d4	14a6b1d0-f886-4f46-9166-a134668d16ab	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:53:51.14386	\N	\N	\N	\N	f
2c55239e-493d-497f-97ea-5c6aba317e33	1	3d8be820-f83f-4579-b8e2-a8c4b5465d69	14a6b1d0-f886-4f46-9166-a134668d16ab	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:53:51.145604	\N	\N	\N	\N	f
2c6f40e1-a56c-4b12-a8ff-18f2f27e2dd7	1	22e64c46-97c3-40a7-a4aa-4b11eb838446	39ad1877-9e15-4498-88bb-ef6d617a23d2	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 21:53:51.145558	\N	\N	\N	\N	f
2d072168-de45-404d-8190-9d5e20462be2	1	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	14a6b1d0-f886-4f46-9166-a134668d16ab	15d219ed-b4eb-46de-9f55-741dd7dcec62	2024-10-19 21:53:51.145954	\N	\N	\N	\N	f
3e2f8319-6035-4479-8831-9797b434c747	1	3652e96a-9dc0-4f12-817c-1ca7f05807e6	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 21:53:51.146056	\N	\N	\N	\N	f
3f57bb2d-ac90-4730-9cf9-11e4c50ea059	1	e21d9b47-d1bb-4c02-9704-acff338cf963	384d21de-6a0f-4c92-b0ef-540ff97079bc	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 21:53:51.145482	\N	\N	\N	\N	f
47788bcf-a577-44df-9f78-0154788fcaab	1	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	6b8b0603-8e07-4181-92ec-ee13f0e768ce	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 21:53:51.145535	\N	\N	\N	\N	f
4fed3c46-b9ba-4f59-afd5-8df4baa30595	1	84609dec-b050-496e-81be-301a1334919a	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:53:51.146005	\N	\N	\N	\N	f
51aafb5e-d6d6-460d-96d0-6540c3ed6ecc	1	83c97377-4790-4e12-9b61-c0456fe642b2	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:53:51.146042	\N	\N	\N	\N	f
57d4a268-b921-4a37-b2b6-01a9bc69d098	1	7374bc88-8afb-4236-9fa0-d75adad253a0	5f55d75a-ca3a-4375-bdc4-afb591aef21d	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:53:51.145768	\N	\N	\N	\N	f
58b489da-ab0b-4641-ac3a-c7fa16581e53	1	2fa772f8-0fa4-472b-a154-14cf794d50e2	e095bbae-68d2-4077-9036-697c526736d4	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 21:53:51.145582	\N	\N	\N	\N	f
6846831e-07a9-4464-be4b-b718bd8e365c	1	84609dec-b050-496e-81be-301a1334919a	be26aee1-0512-4e6a-8313-5c18759958a9	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:53:51.145741	\N	\N	\N	\N	f
68b5d4fa-9f48-4926-ba11-d03416646f12	1	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	d1372bba-be85-473c-8086-02a7c9890140	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 21:53:51.145729	\N	\N	\N	\N	f
7c0ad8dc-c2a5-42ca-b92f-9da1093f734a	1	978e2b3f-9c26-41f0-b3c4-cba2e492767f	439c9800-35c9-48ee-8549-9c293a107743	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:53:51.145464	\N	\N	\N	\N	f
85c870fb-dbbf-400a-86e9-d0e7b5ee4fcd	1	18e845d8-400b-4d12-a414-9cd440f92677	72843603-7dc4-4405-92fa-9a7289ac9b66	ecfbb998-043f-40d3-af8c-c0a0cf04f57b	2024-10-19 21:53:51.145994	\N	\N	\N	\N	f
86dc5075-06c9-4eae-b361-645c6a875dd8	1	14baebc0-0189-423c-a14c-d62ffe981f63	e00c9a01-ea24-48db-ac41-4d39c79f9321	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 21:53:51.145804	\N	\N	\N	\N	f
8e208ca3-a60d-4164-ad2a-d8db942104e4	1	00c05513-4129-4aa6-b79e-983ff13574fc	7b42cb26-668a-4037-8ffc-68058704a460	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:53:51.145593	\N	\N	\N	\N	f
90677776-7c21-4079-99d3-261b7e7df552	1	d1372bba-be85-473c-8086-02a7c9890140	84609dec-b050-496e-81be-301a1334919a	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:53:51.145714	\N	\N	\N	\N	f
9bc3da60-7b88-4269-a271-d340ce99896a	1	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	8f722abd-0123-4494-b71c-a21943484a3c	b7594574-0d60-4ffa-b14d-5917c719889b	2024-10-19 21:53:51.145826	\N	\N	\N	\N	f
9bec7538-2077-42f6-9176-038ff9e87c29	1	00c05513-4129-4aa6-b79e-983ff13574fc	ed964db3-afac-426e-8988-c2ed54a89510	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:53:51.145942	\N	\N	\N	\N	f
a82e314e-6845-4032-a7fc-7098841317ed	1	7374bc88-8afb-4236-9fa0-d75adad253a0	143437a3-503e-4e95-911d-4c6571ddea8e	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:53:51.145965	\N	\N	\N	\N	f
ad121b51-287c-4382-bc28-8aa6baffe00d	1	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	9ca9bcee-c97f-4778-83f4-57fff49759d1	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 21:53:51.145677	\N	\N	\N	\N	f
b079e390-3307-4715-b990-7b56ea3ce938	1	33725381-a183-49ef-b723-e70495ff6066	14a6b1d0-f886-4f46-9166-a134668d16ab	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:53:51.145517	\N	\N	\N	\N	f
b0a5fdc3-773b-4d5b-8f29-f1e1ec5abf00	1	b55f5bbd-4b95-448a-b38b-a1429002854b	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:53:51.145448	\N	\N	\N	\N	f
b2357155-acea-41da-9a58-655ac34e1fb5	1	7b42cb26-668a-4037-8ffc-68058704a460	0b996fe8-4582-412b-adfb-6fa402c25bf4	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 21:53:51.146031	\N	\N	\N	\N	f
c693c657-b56f-44d0-9a6d-a0b438910128	1	d1372bba-be85-473c-8086-02a7c9890140	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:53:51.146019	\N	\N	\N	\N	f
cfedf655-b5e4-4b1b-a6bd-95886b30b17a	1	f18bc355-4a5c-4012-89a6-0044e4bfe36f	b3243d6a-7be2-4c83-8a89-dfd4a1976095	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:53:51.145903	\N	\N	\N	\N	f
e2eab3ac-c101-4517-8412-0197d45f1f92	1	6e132241-d674-4195-b8c5-b6b4d320e3ce	22e64c46-97c3-40a7-a4aa-4b11eb838446	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 21:53:51.14579	\N	\N	\N	\N	f
e3043b03-df4b-45c7-8f3f-bf08580111c1	1	9612f20e-6fce-4190-bc29-b31d7d3d9188	be26aee1-0512-4e6a-8313-5c18759958a9	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:53:51.145914	\N	\N	\N	\N	f
ea6ead70-b4a4-4303-bed4-a86b81764381	1	7374bc88-8afb-4236-9fa0-d75adad253a0	e095bbae-68d2-4077-9036-697c526736d4	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:53:51.145546	\N	\N	\N	\N	f
ec69d449-5aa0-4732-8290-7d677bb04f4c	1	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	1cc85c40-c092-4bee-adeb-3dc17e304563	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:53:51.145852	\N	\N	\N	\N	f
ed286a08-6bab-4e85-87a5-0b2f92cf0635	1	9ca9bcee-c97f-4778-83f4-57fff49759d1	20105f5a-82e0-4763-b12c-7a5ddc9abf83	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:53:51.145632	\N	\N	\N	\N	f
ef8d7e65-900e-4b25-963c-e9f7a22c0069	1	134e6153-f93b-4592-8bd7-ae30e9321017	14baebc0-0189-423c-a14c-d62ffe981f63	dc2623fe-8a17-4340-abf9-d51a6e118efc	2024-10-19 21:53:51.14566	\N	\N	\N	\N	f
efdcea2f-8454-4e6f-9681-a1e53d5bf624	1	33725381-a183-49ef-b723-e70495ff6066	9f64a38d-8cdd-4a21-a529-9747a9331998	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:53:51.145926	\N	\N	\N	\N	f
f2d063c4-78f3-401a-9b84-7bf35b2a48d7	1	eba19f8f-0936-45eb-88bc-9c83772a1d93	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 21:53:51.145876	\N	\N	\N	\N	f
f492afe3-5bea-4b3a-bdab-4ff740efeb80	1	33725381-a183-49ef-b723-e70495ff6066	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:53:51.145691	\N	\N	\N	\N	f
f6296f74-1113-485e-8ff3-34b89270e041	1	0b996fe8-4582-412b-adfb-6fa402c25bf4	b55f5bbd-4b95-448a-b38b-a1429002854b	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:53:51.146079	\N	\N	\N	\N	f
f72e0202-6c3f-4607-b316-50f978e0661a	1	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 21:53:51.14584	\N	\N	\N	\N	f
f9ad5154-4661-458a-a796-4223d24e0754	1	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	8b92673a-ba81-4629-aea9-41444a46a0dc	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 21:53:51.145702	\N	\N	\N	\N	f
\.


--
-- TOC entry 2982 (class 0 OID 16748)
-- Dependencies: 205
-- Data for Name: interactions; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.interactions (id, type, user_profile_id, relate_profile_id, friendship_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
1ff74f87-9b60-480e-92b2-3261230488bf	0	e095bbae-68d2-4077-9036-697c526736d4	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:53:51.265554	\N	\N	\N	\N	f
3432f0c0-5ba1-4d5d-841f-66f70561bb51	0	83c97377-4790-4e12-9b61-c0456fe642b2	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	\N	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:53:51.265348	\N	\N	\N	\N	f
92df6758-dbe0-4c06-8e7b-5731f49498f2	0	00c05513-4129-4aa6-b79e-983ff13574fc	7b42cb26-668a-4037-8ffc-68058704a460	\N	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:53:51.265602	\N	\N	\N	\N	f
9df3c383-a0ed-4af0-a6a4-31002910f7e4	0	0b996fe8-4582-412b-adfb-6fa402c25bf4	b55f5bbd-4b95-448a-b38b-a1429002854b	\N	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:53:51.265565	\N	\N	\N	\N	f
d9a4b420-9115-4e8b-8090-5f5f26d3ccbe	0	eb1b0535-b7f3-430e-b91c-c1feea653f5f	9612f20e-6fce-4190-bc29-b31d7d3d9188	\N	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 21:53:51.265542	\N	\N	\N	\N	f
dfec6ebe-ff69-4ff1-aef4-f1d9b7734a7d	0	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	\N	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 21:53:51.265579	\N	\N	\N	\N	f
f3efca80-b466-46d8-8b61-74168a8f4fff	0	33725381-a183-49ef-b723-e70495ff6066	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:53:51.26559	\N	\N	\N	\N	f
146ac49e-3436-4ae4-baaa-55ffb27fabe6	0	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	3a8a6697-bc5c-4f42-8db6-82153212aeea	\N	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:53:51.305797	\N	\N	\N	\N	f
4f3d6f8a-0f7f-4743-b662-f96292f179d7	0	4929722e-df51-411e-8c00-59955f7d8fd8	6c51e2a8-b2dc-40f1-8586-eb4119083ecf	\N	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 21:53:51.305823	\N	\N	\N	\N	f
4f5d01b4-a693-4c20-b0d4-90def8d216e8	0	45370c44-1d4d-4834-8cd5-3551b5d30199	9e612e81-041e-4ba9-9b86-60851ab3b2fb	\N	d34efe03-6baf-42df-8e7b-0418ac94c7f8	2024-10-19 21:53:51.305956	\N	\N	\N	\N	f
63d7b5fe-8cd2-4aa7-b6c1-9022eed562a2	0	7374bc88-8afb-4236-9fa0-d75adad253a0	3eb11e9f-dedd-47f5-b22f-3cd3644da12f	\N	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:53:51.305896	\N	\N	\N	\N	f
649cb73c-aaa0-43d4-8f02-1d411342c1f9	0	143437a3-503e-4e95-911d-4c6571ddea8e	148e75f7-bb0e-4719-be0a-45273321600b	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 21:53:51.30586	\N	\N	\N	\N	f
712317fa-eb11-44e5-bd63-7c00849204e6	0	143437a3-503e-4e95-911d-4c6571ddea8e	1518f0d5-f074-42e5-80b9-22be2d74ac5a	\N	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 21:53:51.305848	\N	\N	\N	\N	f
72d59552-1bd3-4191-8c6b-62514bfec803	0	3d8be820-f83f-4579-b8e2-a8c4b5465d69	76a183b5-ebfc-4311-adb8-c40449580bf5	\N	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:53:51.305932	\N	\N	\N	\N	f
772a5303-90d1-444f-9acd-c077e36858a0	0	be26aee1-0512-4e6a-8313-5c18759958a9	8ffc9ff6-bb90-4ade-931d-f111075fcc07	\N	c0f9ff94-a28c-4e9c-a2c7-720aca11e966	2024-10-19 21:53:51.305968	\N	\N	\N	\N	f
9e806391-43ab-407f-8371-7c815f31cb46	0	5f55d75a-ca3a-4375-bdc4-afb591aef21d	d1af73b6-6733-4e37-85d9-4de32d2a51c2	\N	aa223821-cdb2-4061-a4bc-55c2ef4f69d0	2024-10-19 21:53:51.305871	\N	\N	\N	\N	f
b145f50c-0162-4af9-bcaf-2e2d685f53a7	0	1cc85c40-c092-4bee-adeb-3dc17e304563	f0809579-4622-4812-9da3-6b583378dfa8	\N	3d3cb675-d596-49aa-89af-61479d8c8e8d	2024-10-19 21:53:51.305812	\N	\N	\N	\N	f
b8e9b699-d630-4b10-b02f-31f1d48c0386	0	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	e9dd5ea5-b5c2-4fc2-a37e-3bf20bf1890c	\N	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 21:53:51.305728	\N	\N	\N	\N	f
ccad47a5-8bab-4d1e-a38a-02ec0fecf503	0	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	f71ef52d-5e00-443e-8cb5-b896e00e0057	\N	69914b7d-a41b-43ff-9419-b86ddc8d5cb1	2024-10-19 21:53:51.305834	\N	\N	\N	\N	f
d556f4ad-a261-4c61-8198-c3097fdf4667	0	7374bc88-8afb-4236-9fa0-d75adad253a0	1518f0d5-f074-42e5-80b9-22be2d74ac5a	\N	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:53:51.305943	\N	\N	\N	\N	f
df6e606c-9a4f-490d-b161-0bda09d05ce1	0	be26aee1-0512-4e6a-8313-5c18759958a9	e9dd5ea5-b5c2-4fc2-a37e-3bf20bf1890c	\N	c0f9ff94-a28c-4e9c-a2c7-720aca11e966	2024-10-19 21:53:51.305884	\N	\N	\N	\N	f
e53ad50a-7d1c-46ff-83fe-7690d3b95df0	0	49fa1298-7d26-4de1-b197-3005c3d03c0e	76a183b5-ebfc-4311-adb8-c40449580bf5	\N	88ea9d8d-9bf0-40ed-a794-32835eac461a	2024-10-19 21:53:51.305907	\N	\N	\N	\N	f
eac74bbc-bb71-42e3-9a68-9b30dcd7545e	0	b55f5bbd-4b95-448a-b38b-a1429002854b	9aefd7eb-c580-4ac2-a72c-a36f12b10a92	\N	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:53:51.305785	\N	\N	\N	\N	f
f95c6b8d-2dd0-40e3-9842-6d20ca6054fe	0	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	4fe0e442-5a58-4f9f-941d-833bf02f3339	\N	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:53:51.305921	\N	\N	\N	\N	f
\.

--
-- TOC entry 2848 (class 2606 OID 16747)
-- Name: friendships PK_friendships; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT "PK_friendships" PRIMARY KEY (id);


--
-- TOC entry 2852 (class 2606 OID 16752)
-- Name: interactions PK_interactions; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.interactions
    ADD CONSTRAINT "PK_interactions" PRIMARY KEY (id);


--
-- TOC entry 2846 (class 1259 OID 16758)
-- Name: IX_friendships_sender_id_receiver_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_friendships_sender_id_receiver_id" ON public.friendships USING btree (sender_id, receiver_id);


--
-- TOC entry 2849 (class 1259 OID 16759)
-- Name: IX_interactions_friendship_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_interactions_friendship_id" ON public.interactions USING btree (friendship_id);


--
-- TOC entry 2850 (class 1259 OID 16760)
-- Name: IX_interactions_user_profile_id_relate_profile_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_interactions_user_profile_id_relate_profile_id" ON public.interactions USING btree (user_profile_id, relate_profile_id);


--
-- TOC entry 2853 (class 2606 OID 16753)
-- Name: interactions FK_interactions_friendships_friendship_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.interactions
    ADD CONSTRAINT "FK_interactions_friendships_friendship_id" FOREIGN KEY (friendship_id) REFERENCES public.friendships(id) ON DELETE CASCADE;


-- Completed on 2024-10-19 16:00:14 UTC

--
-- PostgreSQL database dump complete
--

