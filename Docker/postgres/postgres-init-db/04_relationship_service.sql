\c relationship_service_db
--
-- PostgreSQL database dump
--

-- Dumped from database version 12.20 (Debian 12.20-1.pgdg110+1)
-- Dumped by pg_dump version 12.20

-- Started on 2024-10-26 17:25:10 UTC

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
-- TOC entry 2981 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 16761)
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
-- TOC entry 204 (class 1259 OID 16766)
-- Name: interactions; Type: TABLE; Schema: public; Owner: infinitynetUser
--

CREATE TABLE public.interactions (
    id uuid NOT NULL,
    type integer NOT NULL,
    profile_id uuid NOT NULL,
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
-- TOC entry 2974 (class 0 OID 16761)
-- Dependencies: 203
-- Data for Name: friendships; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.friendships (id, status, sender_id, receiver_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
027a6d22-bd6c-45b8-a9eb-7d12a6236e8b	1	2e6b7127-5e54-43eb-a21f-64c57143824d	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-26 23:23:21.490666	\N	\N	\N	\N	f
0a2f9d63-08e0-4e0d-8fa5-4130375a9f2a	1	00c05513-4129-4aa6-b79e-983ff13574fc	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	00c05513-4129-4aa6-b79e-983ff13574fc	2024-10-26 23:23:21.500849	\N	\N	\N	\N	f
0e04d57a-385d-4f30-b407-299330a02428	1	2e6b7127-5e54-43eb-a21f-64c57143824d	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-26 23:23:21.489826	\N	\N	\N	\N	f
0f0e5820-a7de-47ea-8e96-ad73454060c0	1	14baebc0-0189-423c-a14c-d62ffe981f63	b6663ea1-57ec-4c3a-9597-da421b3c9484	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-26 23:23:21.501388	\N	\N	\N	\N	f
11aef387-4673-4c36-a16e-f1784d617184	1	b6663ea1-57ec-4c3a-9597-da421b3c9484	e21d9b47-d1bb-4c02-9704-acff338cf963	b6663ea1-57ec-4c3a-9597-da421b3c9484	2024-10-26 23:23:21.499024	\N	\N	\N	\N	f
1c022dd9-23ab-423c-8ef8-1be2cb44d83a	1	83c97377-4790-4e12-9b61-c0456fe642b2	612e214e-4fe6-4b17-b9af-8b8b26bf180e	83c97377-4790-4e12-9b61-c0456fe642b2	2024-10-26 23:23:21.497248	\N	\N	\N	\N	f
1d4cffcd-9118-4e1f-9d8d-d67b02651ba4	1	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	d1372bba-be85-473c-8086-02a7c9890140	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-26 23:23:21.493074	\N	\N	\N	\N	f
214283f9-7371-4e45-8b5b-19c5086ce42e	1	74d9ea46-5729-454f-b994-8faee093ddef	5f55d75a-ca3a-4375-bdc4-afb591aef21d	74d9ea46-5729-454f-b994-8faee093ddef	2024-10-26 23:23:21.488967	\N	\N	\N	\N	f
231e4cb5-2ea8-4855-9546-d97fdfd9c04c	1	35d0da5e-7492-46d3-aaca-17455a353de9	e095bbae-68d2-4077-9036-697c526736d4	35d0da5e-7492-46d3-aaca-17455a353de9	2024-10-26 23:23:21.50863	\N	\N	\N	\N	f
262fa503-2626-4de9-b67a-6ca539e9cb5f	1	6700632c-6c3b-4d7e-81dd-8b2151b60502	50088da9-86e5-4781-be1e-f8b04a2554d0	6700632c-6c3b-4d7e-81dd-8b2151b60502	2024-10-26 23:23:21.500265	\N	\N	\N	\N	f
2bc6a8ba-18f6-4c0c-8d59-ef83b1753c91	1	384d21de-6a0f-4c92-b0ef-540ff97079bc	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	384d21de-6a0f-4c92-b0ef-540ff97079bc	2024-10-26 23:23:21.485214	\N	\N	\N	\N	f
32242c11-c16b-4ab0-a179-0a9c53bf2862	1	33725381-a183-49ef-b723-e70495ff6066	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	33725381-a183-49ef-b723-e70495ff6066	2024-10-26 23:23:21.493513	\N	\N	\N	\N	f
3398a4a6-090d-409a-af9c-9c7196805a67	1	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	978e2b3f-9c26-41f0-b3c4-cba2e492767f	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2024-10-26 23:23:21.488123	\N	\N	\N	\N	f
34377362-a34e-4b93-b90d-a78d5ac05299	1	b55f5bbd-4b95-448a-b38b-a1429002854b	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	b55f5bbd-4b95-448a-b38b-a1429002854b	2024-10-26 23:23:21.492514	\N	\N	\N	\N	f
35bf83e1-8c3c-459f-80bc-21bb98ac0d46	1	959b7d55-62bf-42c0-a313-33054551abb5	7b42cb26-668a-4037-8ffc-68058704a460	959b7d55-62bf-42c0-a313-33054551abb5	2024-10-26 23:23:21.503751	\N	\N	\N	\N	f
41960d20-03dd-41a9-9436-bd78b0b0af92	1	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	07f86036-511f-47d1-b7b7-4543b2eb3303	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	2024-10-26 23:23:21.506112	\N	\N	\N	\N	f
4786a15c-905b-43a3-a736-5a809f0a4b82	1	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	53453386-8816-485f-9a08-22c07cf29d22	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	2024-10-26 23:23:21.480155	\N	\N	\N	\N	f
4d542333-3448-4545-acff-e97320235106	1	2e6b7127-5e54-43eb-a21f-64c57143824d	8f722abd-0123-4494-b71c-a21943484a3c	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-26 23:23:21.489415	\N	\N	\N	\N	f
519cb1b2-a37f-4c98-b720-863b0bc4d71b	1	705391da-77b5-4f08-b176-301a5f1bbc0d	be26aee1-0512-4e6a-8313-5c18759958a9	705391da-77b5-4f08-b176-301a5f1bbc0d	2024-10-26 23:23:21.502517	\N	\N	\N	\N	f
66004595-8f7e-4950-bc2b-cd5a0a35f2b4	1	0b996fe8-4582-412b-adfb-6fa402c25bf4	439c9800-35c9-48ee-8549-9c293a107743	0b996fe8-4582-412b-adfb-6fa402c25bf4	2024-10-26 23:23:21.483902	\N	\N	\N	\N	f
7a75a077-03e4-42c9-a93e-1ae7f56ebc0e	1	39ad1877-9e15-4498-88bb-ef6d617a23d2	fadd55dc-c457-41a6-9723-c259182f0035	39ad1877-9e15-4498-88bb-ef6d617a23d2	2024-10-26 23:23:21.484352	\N	\N	\N	\N	f
82303e12-f1de-46b7-99d1-6e48eaff6dc7	1	fe1e460d-16ac-4fcd-b512-2413b8cb3256	959b7d55-62bf-42c0-a313-33054551abb5	fe1e460d-16ac-4fcd-b512-2413b8cb3256	2024-10-26 23:23:21.496113	\N	\N	\N	\N	f
8b5fa255-8629-47bb-aaca-3b94ad0efea0	1	c6d25490-d32a-410d-be77-5370cc1482a2	5f55d75a-ca3a-4375-bdc4-afb591aef21d	c6d25490-d32a-410d-be77-5370cc1482a2	2024-10-26 23:23:21.486938	\N	\N	\N	\N	f
8ccd1f52-b16f-46de-8d9d-0d459fcf00fb	1	2fa772f8-0fa4-472b-a154-14cf794d50e2	14baebc0-0189-423c-a14c-d62ffe981f63	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-26 23:23:21.491072	\N	\N	\N	\N	f
954db803-1bf5-4c86-b397-5eaa60bd0e52	1	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	c6d25490-d32a-410d-be77-5370cc1482a2	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	2024-10-26 23:23:21.487715	\N	\N	\N	\N	f
963355ac-c2e9-4088-8880-acd082eb7c1c	1	bb05cc9c-87a1-4d43-b253-d172e2117ff2	2e6b7127-5e54-43eb-a21f-64c57143824d	bb05cc9c-87a1-4d43-b253-d172e2117ff2	2024-10-26 23:23:21.495345	\N	\N	\N	\N	f
a959c438-2a31-4555-bcff-4114dd432fc1	1	5f55d75a-ca3a-4375-bdc4-afb591aef21d	143437a3-503e-4e95-911d-4c6571ddea8e	5f55d75a-ca3a-4375-bdc4-afb591aef21d	2024-10-26 23:23:21.509974	\N	\N	\N	\N	f
b433751e-ef6e-4c4c-b792-c1ecda730085	1	2e6b7127-5e54-43eb-a21f-64c57143824d	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-26 23:23:21.504323	\N	\N	\N	\N	f
bd1b3745-d73c-42b0-b650-3b4a3a0f52d2	1	2e6b7127-5e54-43eb-a21f-64c57143824d	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2e6b7127-5e54-43eb-a21f-64c57143824d	2024-10-26 23:23:21.494884	\N	\N	\N	\N	f
c2526987-b3c8-48d0-bdcf-bf622a3fcf1d	1	fa846317-fe54-4f52-b8d6-6a618387a5da	1f981aae-f40b-4dba-b383-8853d87b23c5	fa846317-fe54-4f52-b8d6-6a618387a5da	2024-10-26 23:23:21.49393	\N	\N	\N	\N	f
c25c6c25-fbdb-4080-8fe8-1390dca9f17e	1	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	2024-10-26 23:23:21.501946	\N	\N	\N	\N	f
c7e8a1e3-57a9-4bd1-8390-94af1235e1d0	1	fadd55dc-c457-41a6-9723-c259182f0035	74d9ea46-5729-454f-b994-8faee093ddef	fadd55dc-c457-41a6-9723-c259182f0035	2024-10-26 23:23:21.499655	\N	\N	\N	\N	f
c8b37663-11ba-4f31-8fc5-b1424935318e	1	3d8be820-f83f-4579-b8e2-a8c4b5465d69	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-26 23:23:21.498435	\N	\N	\N	\N	f
c975fd51-253b-469e-b582-583f78f6b20e	1	20105f5a-82e0-4763-b12c-7a5ddc9abf83	14a6b1d0-f886-4f46-9166-a134668d16ab	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-26 23:23:21.49024	\N	\N	\N	\N	f
d02ab9b9-0d5b-4698-96e4-953dd58a89b3	1	b1469423-4113-490e-bcd6-b79a146c3f81	2eb2ae7e-b05a-45c8-83ef-a23717e17947	b1469423-4113-490e-bcd6-b79a146c3f81	2024-10-26 23:23:21.509361	\N	\N	\N	\N	f
d13b9513-4f7c-4898-b93e-22d2cbd54c12	1	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-26 23:23:21.504917	\N	\N	\N	\N	f
e5d65bf0-77f0-43d5-b8ac-8fb9e36f3e04	1	2fa772f8-0fa4-472b-a154-14cf794d50e2	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-26 23:23:21.488553	\N	\N	\N	\N	f
e62b118e-2243-4112-8835-17a3a295b411	1	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	35d0da5e-7492-46d3-aaca-17455a353de9	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2024-10-26 23:23:21.503164	\N	\N	\N	\N	f
ed4bd0ca-4411-4875-91cf-25e3db8e688d	1	978e2b3f-9c26-41f0-b3c4-cba2e492767f	1f981aae-f40b-4dba-b383-8853d87b23c5	978e2b3f-9c26-41f0-b3c4-cba2e492767f	2024-10-26 23:23:21.494347	\N	\N	\N	\N	f
ee76762a-00af-4aa2-9e99-d571879ccd0f	1	3d8be820-f83f-4579-b8e2-a8c4b5465d69	14baebc0-0189-423c-a14c-d62ffe981f63	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-26 23:23:21.505521	\N	\N	\N	\N	f
f1f997bb-9406-4bce-989b-e0cebb5649cc	1	be26aee1-0512-4e6a-8313-5c18759958a9	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	be26aee1-0512-4e6a-8313-5c18759958a9	2024-10-26 23:23:21.485955	\N	\N	\N	\N	f
f866dbf0-eeea-4a48-af8a-7ffde2d5e86e	1	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	6e132241-d674-4195-b8c5-b6b4d320e3ce	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	2024-10-26 23:23:21.496679	\N	\N	\N	\N	f
fff02662-459f-4dbd-aa9c-5fee4b1980cd	1	e21d9b47-d1bb-4c02-9704-acff338cf963	2e6b7127-5e54-43eb-a21f-64c57143824d	e21d9b47-d1bb-4c02-9704-acff338cf963	2024-10-26 23:23:21.497851	\N	\N	\N	\N	f
\.


--
-- TOC entry 2975 (class 0 OID 16766)
-- Dependencies: 204
-- Data for Name: interactions; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.interactions (id, type, profile_id, relate_profile_id, friendship_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
1dc34428-1bdd-4e01-a986-378e9bca50d6	0	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-26 23:23:21.737803	\N	\N	\N	\N	f
1f57caa2-c04f-469f-9188-1b85a052c542	0	3d8be820-f83f-4579-b8e2-a8c4b5465d69	14baebc0-0189-423c-a14c-d62ffe981f63	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-26 23:23:21.732957	\N	\N	\N	\N	f
258c6d78-d854-4890-a4e0-8dd3f06424e0	0	6700632c-6c3b-4d7e-81dd-8b2151b60502	50088da9-86e5-4781-be1e-f8b04a2554d0	\N	6700632c-6c3b-4d7e-81dd-8b2151b60502	2024-10-26 23:23:21.736516	\N	\N	\N	\N	f
3240c4f8-14ad-42fd-a331-466c5f052ab9	0	2fa772f8-0fa4-472b-a154-14cf794d50e2	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	\N	2fa772f8-0fa4-472b-a154-14cf794d50e2	2024-10-26 23:23:21.737116	\N	\N	\N	\N	f
5e1b108a-96d5-43d7-9930-527293e6e914	0	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	d1372bba-be85-473c-8086-02a7c9890140	\N	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-26 23:23:21.734581	\N	\N	\N	\N	f
77fbbe02-1f2c-484a-adc0-6266e7b1e47a	0	3d8be820-f83f-4579-b8e2-a8c4b5465d69	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	\N	3d8be820-f83f-4579-b8e2-a8c4b5465d69	2024-10-26 23:23:21.735806	\N	\N	\N	\N	f
883fe8b7-77a4-475f-a31c-586946fb5d2c	0	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	978e2b3f-9c26-41f0-b3c4-cba2e492767f	\N	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	2024-10-26 23:23:21.738423	\N	\N	\N	\N	f
88d7eb3c-69d9-4ab5-a95d-f6159bd02059	0	fe1e460d-16ac-4fcd-b512-2413b8cb3256	959b7d55-62bf-42c0-a313-33054551abb5	\N	fe1e460d-16ac-4fcd-b512-2413b8cb3256	2024-10-26 23:23:21.740207	\N	\N	\N	\N	f
a2cdd9bc-cdf3-432a-83bf-4786c6986b3f	0	fadd55dc-c457-41a6-9723-c259182f0035	74d9ea46-5729-454f-b994-8faee093ddef	\N	fadd55dc-c457-41a6-9723-c259182f0035	2024-10-26 23:23:21.739035	\N	\N	\N	\N	f
c64a5559-a04e-47d7-8fca-c8796a684d8d	0	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	07f86036-511f-47d1-b7b7-4543b2eb3303	\N	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	2024-10-26 23:23:21.733694	\N	\N	\N	\N	f
d0bb8e82-ebed-486b-a6a0-59bebcb43196	0	33725381-a183-49ef-b723-e70495ff6066	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	\N	33725381-a183-49ef-b723-e70495ff6066	2024-10-26 23:23:21.732098	\N	\N	\N	\N	f
d2ac0dd5-f776-4744-9a28-0afee00bbe74	0	14baebc0-0189-423c-a14c-d62ffe981f63	b6663ea1-57ec-4c3a-9597-da421b3c9484	\N	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-26 23:23:21.739633	\N	\N	\N	\N	f
dc2aa3f1-2e30-44ea-b91f-ac1e0bbd778a	0	20105f5a-82e0-4763-b12c-7a5ddc9abf83	14a6b1d0-f886-4f46-9166-a134668d16ab	\N	20105f5a-82e0-4763-b12c-7a5ddc9abf83	2024-10-26 23:23:21.735169	\N	\N	\N	\N	f
0766efa0-0afa-469e-89b1-fbf3f4a2cc1a	0	439c9800-35c9-48ee-8549-9c293a107743	8ffc9ff6-bb90-4ade-931d-f111075fcc07	\N	439c9800-35c9-48ee-8549-9c293a107743	2024-10-26 23:23:21.844284	\N	\N	\N	\N	f
1eade696-2e09-45a3-8e8c-a4539f4e9147	0	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	ab0d3ab7-3b7c-4054-b042-4cc2303d25f1	\N	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	2024-10-26 23:23:21.842406	\N	\N	\N	\N	f
316eb854-7c11-4523-9d1c-c81621de0585	0	33725381-a183-49ef-b723-e70495ff6066	91ea986e-c6bf-45db-bdb6-8cf01428fef8	\N	33725381-a183-49ef-b723-e70495ff6066	2024-10-26 23:23:21.841091	\N	\N	\N	\N	f
3e0478a8-8bc1-4268-af29-d7a135db44e9	0	8f722abd-0123-4494-b71c-a21943484a3c	3b9f10c5-e0fd-4a61-a745-87b3dce29887	\N	8f722abd-0123-4494-b71c-a21943484a3c	2024-10-26 23:23:21.838751	\N	\N	\N	\N	f
6c7e0f56-3994-4cb3-b015-67461982426a	0	14baebc0-0189-423c-a14c-d62ffe981f63	26d6acc1-7fba-4e24-8be2-d0c3e30c61dc	\N	14baebc0-0189-423c-a14c-d62ffe981f63	2024-10-26 23:23:21.835159	\N	\N	\N	\N	f
7ed2e276-9477-4243-892d-14da25d61bec	0	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	18ca718a-ac1c-4c95-99fa-952018c45180	\N	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	2024-10-26 23:23:21.840434	\N	\N	\N	\N	f
88424b5a-cf0a-4445-b369-295b2267a9c0	0	6e132241-d674-4195-b8c5-b6b4d320e3ce	8b435e2a-3fbb-42ab-8644-ce5d40d85643	\N	6e132241-d674-4195-b8c5-b6b4d320e3ce	2024-10-26 23:23:21.834529	\N	\N	\N	\N	f
8a67fbfa-d892-4d05-8ea7-a6d37a956446	0	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	6c51e2a8-b2dc-40f1-8586-eb4119083ecf	\N	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	2024-10-26 23:23:21.838175	\N	\N	\N	\N	f
941255e4-ff99-419b-96e2-88179289be39	0	b3243d6a-7be2-4c83-8a89-dfd4a1976095	8b435e2a-3fbb-42ab-8644-ce5d40d85643	\N	b3243d6a-7be2-4c83-8a89-dfd4a1976095	2024-10-26 23:23:21.835769	\N	\N	\N	\N	f
98392832-80cc-4ee5-b91b-51db0fe49d5f	0	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	8ffc9ff6-bb90-4ade-931d-f111075fcc07	\N	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	2024-10-26 23:23:21.836943	\N	\N	\N	\N	f
a0c0c927-0315-4b49-a4db-89571ef2f78a	0	3652e96a-9dc0-4f12-817c-1ca7f05807e6	6c51e2a8-b2dc-40f1-8586-eb4119083ecf	\N	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2024-10-26 23:23:21.833903	\N	\N	\N	\N	f
b269a45d-0974-4c18-adb9-fdc330e354fc	0	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	10eaaa2a-7b4d-4e6d-8e0c-01b95de6c2de	\N	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	2024-10-26 23:23:21.843022	\N	\N	\N	\N	f
c77dfc92-8684-4675-9787-c14a9e30d6e4	0	1faf9d72-1396-4e99-935d-547b226327c7	4168d4d9-108a-4f5d-8ab9-879acc7defdc	\N	1faf9d72-1396-4e99-935d-547b226327c7	2024-10-26 23:23:21.836344	\N	\N	\N	\N	f
c893841d-9a50-417b-a683-8005a9019173	0	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	7b61e659-5d64-4fe8-b35c-b107d60be4a6	\N	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	2024-10-26 23:23:21.841743	\N	\N	\N	\N	f
ddb206a7-732c-4336-8bc3-d33a5c21a167	0	e21d9b47-d1bb-4c02-9704-acff338cf963	3eb11e9f-dedd-47f5-b22f-3cd3644da12f	\N	e21d9b47-d1bb-4c02-9704-acff338cf963	2024-10-26 23:23:21.833177	\N	\N	\N	\N	f
f2ddb6a9-b598-4acf-92f0-2f57a9e63dce	0	6e132241-d674-4195-b8c5-b6b4d320e3ce	3eb11e9f-dedd-47f5-b22f-3cd3644da12f	\N	6e132241-d674-4195-b8c5-b6b4d320e3ce	2024-10-26 23:23:21.837588	\N	\N	\N	\N	f
f88af1d8-df67-49a1-996e-fd70645c856f	0	9f64a38d-8cdd-4a21-a529-9747a9331998	6c51e2a8-b2dc-40f1-8586-eb4119083ecf	\N	9f64a38d-8cdd-4a21-a529-9747a9331998	2024-10-26 23:23:21.843667	\N	\N	\N	\N	f
\.


--
-- TOC entry 2842 (class 2606 OID 16765)
-- Name: friendships PK_friendships; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT "PK_friendships" PRIMARY KEY (id);


--
-- TOC entry 2846 (class 2606 OID 16770)
-- Name: interactions PK_interactions; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.interactions
    ADD CONSTRAINT "PK_interactions" PRIMARY KEY (id);


--
-- TOC entry 2840 (class 1259 OID 16776)
-- Name: IX_friendships_sender_id_receiver_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_friendships_sender_id_receiver_id" ON public.friendships USING btree (sender_id, receiver_id);


--
-- TOC entry 2843 (class 1259 OID 16777)
-- Name: IX_interactions_friendship_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_interactions_friendship_id" ON public.interactions USING btree (friendship_id);


--
-- TOC entry 2844 (class 1259 OID 16778)
-- Name: IX_interactions_profile_id_relate_profile_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_interactions_profile_id_relate_profile_id" ON public.interactions USING btree (profile_id, relate_profile_id);


--
-- TOC entry 2847 (class 2606 OID 16771)
-- Name: interactions FK_interactions_friendships_friendship_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.interactions
    ADD CONSTRAINT "FK_interactions_friendships_friendship_id" FOREIGN KEY (friendship_id) REFERENCES public.friendships(id) ON DELETE CASCADE;


-- Completed on 2024-10-26 17:25:10 UTC

--
-- PostgreSQL database dump complete
--

