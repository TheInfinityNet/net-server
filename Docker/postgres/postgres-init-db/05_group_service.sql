--
-- PostgreSQL database dump
--

-- Dumped from database version 12.20 (Debian 12.20-1.pgdg110+1)
-- Dumped by pg_dump version 12.20

-- Started on 2024-10-19 16:01:53 UTC
\c group_service_db

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
-- TOC entry 2987 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 204 (class 1259 OID 16613)
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
-- TOC entry 205 (class 1259 OID 16621)
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
-- TOC entry 2980 (class 0 OID 16613)
-- Dependencies: 204
-- Data for Name: groups; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.groups (id, name, description, owner_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
01b040e9-784f-4edc-a439-2996df603eae	Placeat qui id.	Qui eius autem esse deserunt ipsum animi vitae ipsum repellat expedita impedit velit voluptatem et ad dolores alias saepe qui amet sit non veniam non et necessitatibus unde vel sit aut ipsum molestiae voluptas dolores et nobis placeat non occaecati necessitatibus sequi qui perferendis dolorem eum qui hic et dicta.	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:30.958811	\N	\N	\N	\N	f
054e6a0d-2b3b-4286-b4de-d02ed666ab83	Possimus incidunt distinctio.	Corrupti ut id sequi exercitationem libero omnis et et id sit tempora sint neque labore dicta quisquam non nihil corporis fugiat aliquid iure enim tempora quidem quod quod suscipit ipsam a ipsam est neque nisi placeat voluptate impedit fugiat voluptatem laborum rerum omnis quia est ut nulla reprehenderit delectus praesentium.	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:30.958673	\N	\N	\N	\N	f
095f9791-d050-4330-906b-0647ea1786f4	Sit temporibus molestiae.	Quod similique et omnis excepturi id assumenda nostrum est aut et et corporis odit at quod quo quia repellat et aut nisi minus temporibus adipisci in maiores consequatur quibusdam soluta quasi non iste voluptas ea id itaque est quis tenetur aut distinctio maiores ut laboriosam eveniet libero a voluptatem nesciunt.	8b92673a-ba81-4629-aea9-41444a46a0dc	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 21:55:30.958774	\N	\N	\N	\N	f
12b52fea-b990-404f-9f77-13d66ec80399	Veniam aut voluptatem.	Id non qui qui recusandae sed dolore ut necessitatibus et aspernatur occaecati omnis et quis aperiam quas ut magnam non ratione ea nostrum minus qui et dolores velit ea occaecati temporibus officiis quis facere cum deleniti alias ad eum sed quos nam itaque natus ut omnis voluptatem suscipit ut ut.	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:30.958723	\N	\N	\N	\N	f
2526b5fd-b6fc-4bd4-a861-5a011bf04db8	Reiciendis voluptates fugit.	Ut voluptatem hic ad doloremque eum at dolore ad omnis numquam ad aperiam amet rem possimus iure consequuntur vero quisquam nam harum commodi repudiandae saepe aut dolorem aspernatur aperiam velit minima aut praesentium autem molestias magnam magnam excepturi esse neque voluptatum sint ducimus repellat deleniti et quaerat facilis porro facilis.	6700632c-6c3b-4d7e-81dd-8b2151b60502	6d48e156-8327-48d6-91d9-61ce20e3125b	2024-10-19 21:55:30.95842	\N	\N	\N	\N	f
26ab939c-c075-43f7-b16e-6a695866d173	Repellat sint ad.	Iure qui voluptas aut corrupti quia ipsam maiores doloribus dicta quas temporibus voluptatibus cupiditate asperiores assumenda minus veritatis ipsum voluptatem officia odit est et neque consequatur omnis quos ullam nihil consequatur voluptatem fuga quas modi aliquid et ut voluptatem sit sed adipisci rerum voluptatem aliquam perspiciatis est libero sit eos.	83c97377-4790-4e12-9b61-c0456fe642b2	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:55:30.959002	\N	\N	\N	\N	f
27b1736a-9ead-4a1a-9156-fa8922470eef	Similique et itaque.	Nesciunt aliquam voluptatem vel iure aut doloribus laborum itaque beatae sit magni enim quos ipsum voluptatem aspernatur suscipit atque et aut ipsum natus reprehenderit qui quos ut perferendis consequatur et magnam deserunt blanditiis minima optio rerum consequuntur aliquam fugiat eius et expedita natus nesciunt illo aliquam quo autem nostrum inventore.	7374bc88-8afb-4236-9fa0-d75adad253a0	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:55:30.958965	\N	\N	\N	\N	f
2833a7bd-f4a6-472c-9678-883fc2fcda7f	Et dolores aut.	Dolorem ab est quas dolor corporis sint sed optio debitis quasi id itaque dolorum harum qui blanditiis quibusdam sequi aspernatur iusto ut quia sed dolorem cumque numquam facilis corporis sunt placeat sit eaque et aut ipsam odio eos alias tenetur sed ut consectetur rerum fugiat minima hic consequatur quas ea.	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	6af636c4-96e4-4f9e-96a0-794dc6541dc3	2024-10-19 21:55:30.958521	\N	\N	\N	\N	f
295c035f-0873-479b-b155-6746e039e598	Occaecati unde distinctio.	Et quidem ea ut reprehenderit repudiandae voluptatem rem sit maxime commodi soluta est aut nam beatae et laborum quasi ex sit ut est maxime et incidunt autem inventore ut ratione quod aut eveniet non rerum sequi soluta voluptatem repellat illo magnam rerum aut eum delectus unde ipsum nam eius non.	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:30.958585	\N	\N	\N	\N	f
2e68a27c-4347-4b83-91a5-a44e7c47473c	Consequatur dolor est.	Ut voluptas sit vitae veniam voluptatem repudiandae aliquam inventore beatae earum minima laboriosam debitis exercitationem rem sunt harum consequatur voluptatum repudiandae ut repellendus quo qui cupiditate dolorem enim et reiciendis occaecati eos inventore voluptas dolores aut repudiandae suscipit velit vero qui et sunt qui qui molestiae unde quibusdam maiores tenetur.	fa846317-fe54-4f52-b8d6-6a618387a5da	b56dfb50-cf66-498e-80b8-61876a9c4d92	2024-10-19 21:55:30.958976	\N	\N	\N	\N	f
2edf5638-0175-4d58-81fc-92d37118727c	Dicta consequatur sint.	Dolorem non neque libero perferendis vitae qui exercitationem quia odio blanditiis hic ea aspernatur iste eos delectus consequatur amet consequuntur quae quibusdam dignissimos dignissimos sed aliquam perspiciatis dolor vero omnis vel dignissimos sint quia expedita qui maxime molestias fugiat dolore modi iure rerum ea quidem a rerum et itaque ipsa.	6c1fa607-dced-475d-9ad2-1e8ede9071d2	29198ed7-c2be-46cd-a0ed-36bd6a05efbf	2024-10-19 21:55:30.958481	\N	\N	\N	\N	f
32a4c31c-5048-43d8-b8e4-5734f0f6741c	Voluptas soluta incidunt.	Eligendi ab recusandae sunt quia ut in quo excepturi sit eos omnis aut vel accusantium laboriosam aut fugit quia repellat similique enim fugit autem ea ratione iure optio iste saepe ducimus consequatur ducimus enim autem sit perferendis suscipit aut est totam rerum cupiditate enim nihil earum expedita et sed eos.	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	97a977e7-ef81-4b31-9bd2-bd3c065dd17c	2024-10-19 21:55:30.958645	\N	\N	\N	\N	f
3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	Hic modi est.	Optio et sed qui eos vitae debitis neque similique molestiae enim dolorem quod sunt eum reiciendis qui aut nam ipsam reprehenderit blanditiis dolore explicabo qui tenetur ut eveniet voluptatem voluptates aut dolorum non recusandae sed quis suscipit autem tempore optio vel similique aliquid repudiandae sint ad earum culpa saepe fugit.	612e214e-4fe6-4b17-b9af-8b8b26bf180e	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 21:55:30.958917	\N	\N	\N	\N	f
38e635f2-1b68-473b-b941-1e81d253578f	Quia excepturi perspiciatis.	Laudantium aperiam cumque autem nemo rem vitae qui doloribus minima blanditiis cupiditate reiciendis consectetur quibusdam expedita illo quia et dolor consectetur dolorem et aut impedit eaque itaque ex debitis cum rerum qui ad aut quo molestiae sint est expedita sapiente delectus vero possimus qui at sed et aliquam impedit totam.	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 21:55:30.956891	\N	\N	\N	\N	f
39796686-5676-4e48-914a-f405fbead580	Porro quam temporibus.	Atque et sapiente enim ipsum voluptas maxime eveniet vel quis dignissimos cum fugiat ut qui dolorem autem porro et et qui molestiae a vel ullam et impedit dolorum non voluptas earum iure porro et exercitationem eum nam repellat ut et architecto omnis cupiditate ullam modi rerum esse at minus dolorum.	78532cb2-f350-4c98-bce2-e94afd8369c6	4bbe97ff-9028-4030-967e-34d7eae8f332	2024-10-19 21:55:30.958597	\N	\N	\N	\N	f
39d222f0-3b5a-4b40-9c9f-289ae38d61fa	Voluptatum ex voluptatem.	Dolores sed rerum quia doloribus delectus nihil eaque eum quo libero molestias voluptatibus exercitationem ipsum dolor alias consequatur consectetur et dolores sequi voluptates aut dicta suscipit repellendus nobis ea iste dolor dolore nihil qui omnis ipsa provident et est fugiat est blanditiis ratione placeat eaque ipsam dolore officia facilis quae.	9612f20e-6fce-4190-bc29-b31d7d3d9188	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:55:30.959029	\N	\N	\N	\N	f
39da44d5-76ab-47bc-9ff8-121c965e47d5	Laudantium aut quasi.	Quasi ullam deserunt consequuntur molestiae sint ratione tempore excepturi perspiciatis vel tempora mollitia exercitationem voluptatum sed laudantium doloremque nihil quaerat accusantium quisquam ut maxime quasi qui aut laborum est beatae autem ut eum nostrum rerum incidunt minus aut omnis odit qui animi et et sit ut ea aspernatur veniam ea.	612e214e-4fe6-4b17-b9af-8b8b26bf180e	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 21:55:30.959055	\N	\N	\N	\N	f
40e92b5b-736d-4b63-ad95-7dfe2d26bc04	Et suscipit tempora.	Odit suscipit libero et omnis mollitia enim culpa beatae et nemo qui deleniti ab doloremque ipsum animi accusantium voluptatum facere laborum inventore accusantium enim distinctio iste doloremque voluptatem ut animi soluta ut ut ducimus quos doloribus rerum sapiente maiores excepturi quos repellat blanditiis quia et quia sed quo corrupti non.	53453386-8816-485f-9a08-22c07cf29d22	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 21:55:30.958507	\N	\N	\N	\N	f
461d4c74-df9d-4c9c-ba94-e33b19a36d0c	Vitae illo voluptas.	Est et similique id molestias quidem omnis doloribus in laboriosam earum sit corrupti recusandae pariatur fugiat sunt est quia rerum sunt voluptatem sunt quis asperiores sed corporis perferendis a maxime vel quae sit sint incidunt rerum impedit dolorem eos fugit optio nihil aut sed consequatur laborum aut quis ut ut.	384d21de-6a0f-4c92-b0ef-540ff97079bc	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 21:55:30.958762	\N	\N	\N	\N	f
4b38441f-00c1-4f4d-b522-3a33eb89ba5c	Voluptatem sint quis.	Minima facilis maiores animi nemo molestiae hic rerum voluptates iste aliquid ut occaecati maiores eum quia nostrum eum quia nemo velit voluptates corrupti nihil a nisi repudiandae perspiciatis provident aut omnis impedit blanditiis qui perspiciatis neque quam molestiae ut veniam et natus aut officiis et fugit excepturi pariatur quo est.	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:30.958439	\N	\N	\N	\N	f
4c97879a-8899-4a52-9e5a-09b6ab8ade5a	Enim praesentium voluptatem.	Beatae vel ut culpa explicabo itaque quos et molestiae cum amet et vel est quis aut sed nam sequi maxime debitis ea expedita ut sunt recusandae sint odio impedit qui iste aut ea aut modi ut eligendi omnis tempora sed doloremque voluptates recusandae occaecati sed eos voluptatem magni sapiente molestiae.	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 21:55:30.958748	\N	\N	\N	\N	f
4dfeb3d3-2c9b-497e-8384-55e94215571d	Quae aspernatur qui.	Nemo iste ex autem velit id ut repudiandae cupiditate ducimus asperiores ratione aliquid error nostrum eligendi nemo repudiandae provident debitis odit ratione in praesentium hic velit officiis quia illum perspiciatis a quasi eligendi at esse occaecati accusantium optio quo illum laborum quia et repellendus dignissimos aut dolores culpa quis delectus.	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:30.958535	\N	\N	\N	\N	f
5310a3e6-5a16-4090-a632-105cae7d42eb	Qui error neque.	Cupiditate sint voluptatum occaecati nostrum praesentium qui soluta a ut voluptatum praesentium quae impedit sequi ab et sunt et iste nesciunt quis dolores quis eius earum sint odit odio ab laudantium cupiditate quos vitae vero ipsum dolorem rerum quos aut reprehenderit labore quidem doloribus vel aut nulla possimus rem autem.	30d72372-2aee-46cd-ab7f-56dcaefe600c	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 21:55:30.958799	\N	\N	\N	\N	f
68151439-6890-4954-9559-06b02c84acdd	Adipisci praesentium quasi.	Laborum explicabo ea debitis vel adipisci dolor nostrum fugit aut impedit dolorem ut quod inventore veniam sunt est qui dolores est nihil consequatur voluptates ad est eligendi rem facere in debitis perspiciatis id ab velit perspiciatis nihil accusamus odio minima perferendis et neque nostrum quo nobis enim pariatur sint reprehenderit.	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 21:55:30.959041	\N	\N	\N	\N	f
682c7de5-825d-4037-b630-0662381923b7	Quis provident explicabo.	Magni autem omnis consectetur natus assumenda rem dolore aut hic repellat quia eveniet quia amet qui architecto voluptas quis sequi veniam nihil rerum animi sunt qui excepturi consequatur mollitia quis voluptas quaerat suscipit distinctio explicabo incidunt molestiae et nulla facere pariatur sit atque voluptate illo natus recusandae fuga fugiat animi.	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	15d219ed-b4eb-46de-9f55-741dd7dcec62	2024-10-19 21:55:30.958622	\N	\N	\N	\N	f
6b0a3307-50cd-4ab8-b239-52ec9227ff19	Sunt dolores dignissimos.	Et ipsum ratione doloribus est fuga vel voluptatibus vel quia sit cupiditate eaque hic ipsum ad ut omnis hic ut est blanditiis itaque voluptatem voluptatem eos impedit laborum distinctio est nihil fugiat quas perferendis dicta voluptas autem dolorum eaque est saepe iste sint voluptatem magnam accusamus placeat et ipsam quam.	83c97377-4790-4e12-9b61-c0456fe642b2	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:55:30.958565	\N	\N	\N	\N	f
6f51d38c-087b-4b4c-a69e-edb5b593a401	Reprehenderit debitis modi.	Molestiae dolorum minima at non qui maiores sed veritatis esse deleniti velit sed est libero qui mollitia sapiente vero est reprehenderit facere corporis autem dolores dicta qui harum beatae expedita doloremque laudantium hic aut eos laboriosam sapiente voluptas sunt dolor at voluptate harum architecto et molestiae et blanditiis rem dolorem.	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:30.958822	\N	\N	\N	\N	f
71691b66-37b0-4a42-95e2-f6d2c14a7d75	Fugiat quaerat rem.	Sed sed enim ut dolor rem architecto voluptatem quis culpa eligendi qui aut sint dignissimos alias ex occaecati quibusdam maiores qui aliquam ipsum doloribus repellendus dolorem modi ut similique id eius impedit facere saepe inventore deleniti id earum occaecati est modi deleniti est vero modi aut rerum error porro non.	c6d25490-d32a-410d-be77-5370cc1482a2	d776b5c6-c7ca-4d5a-9fd3-6d2828447425	2024-10-19 21:55:30.958685	\N	\N	\N	\N	f
77bedc14-cd28-4fd7-8550-90cab9470ba4	Dignissimos dicta similique.	Sit vitae soluta a aut ducimus accusantium qui iste aut animi ad exercitationem sit nesciunt eligendi eligendi voluptatem enim inventore voluptas eum necessitatibus quod iste asperiores repellat et atque eveniet incidunt minus possimus consequatur rerum atque ex earum est molestiae nobis perspiciatis temporibus velit sunt et dolor libero culpa provident.	09f405ed-f0c6-422c-847f-0e24f7c74aef	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 21:55:30.95894	\N	\N	\N	\N	f
82e8961c-1ae8-4912-b4dc-51173b3fdfe6	Delectus et error.	Quia fugit aut iure magnam velit consequatur illum nostrum quia consequuntur ratione minus ut maiores ut sunt facere excepturi velit qui iure nihil vero quasi dolore maiores repellat autem odio non doloribus nemo et repellat quod tempora molestiae possimus nostrum pariatur dolor quidem distinctio assumenda quam magni et laudantium autem.	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:30.958953	\N	\N	\N	\N	f
83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	Dolores corporis ipsam.	Voluptate delectus totam sit qui rem voluptates blanditiis dolores saepe doloremque ut non nisi porro praesentium cumque voluptas enim quae quasi quis itaque impedit incidunt id et laborum non quae ratione saepe ut ratione quo voluptatem consequatur accusantium modi tempora eveniet quia qui qui atque inventore aliquid fugiat excepturi sed.	384d21de-6a0f-4c92-b0ef-540ff97079bc	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 21:55:30.958737	\N	\N	\N	\N	f
a5c74b1d-9279-441a-a283-8a9a67e6378c	Quibusdam ut accusamus.	Rerum et iusto eveniet officia sunt consequatur unde nesciunt similique non enim accusamus et molestiae molestiae voluptatem porro beatae et accusamus ratione aliquid illum officia autem dolore est consequuntur optio non voluptatem et recusandae dolore consequatur magnam aut ut fugiat alias sint autem ut rem quo esse nobis ut quidem.	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:30.959018	\N	\N	\N	\N	f
a7c48de9-4698-47cf-ad56-cbaaede24885	Animi doloremque soluta.	Possimus est dolorum voluptatibus ad voluptatum eligendi neque porro id ut ducimus nam molestiae iste voluptatum voluptas deleniti error vel sunt ipsa dicta earum veniam in recusandae quam voluptatem aut alias necessitatibus aspernatur dicta est pariatur quisquam tempore tempora nihil molestiae perferendis sed esse error ratione error perferendis non praesentium.	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:30.958634	\N	\N	\N	\N	f
ac641cb5-3883-4fbd-9783-9770175859f1	Veritatis perferendis cupiditate.	Quidem est ut excepturi itaque voluptatem enim vitae accusamus quia nam eveniet dolore vitae dolorem occaecati esse exercitationem dolorem quos eos et ut eius minus quia corporis tenetur et quasi eaque tempora omnis velit quidem est aliquid quidem quo magnam quibusdam quas officiis quae tenetur id molestiae doloribus ipsa ex.	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:30.958469	\N	\N	\N	\N	f
add17a7a-8cdd-47f2-ab30-e237b54ba751	Sint doloribus itaque.	Aliquam et quis temporibus dolorem perspiciatis nam magnam nobis unde laboriosam sed fuga mollitia ut perspiciatis error et corporis vero vitae nulla exercitationem est quia dolore quo saepe nobis eum harum laborum repudiandae a dignissimos iusto in soluta rerum fugiat magni et veritatis consequuntur provident ex inventore quia dolorem praesentium.	be26aee1-0512-4e6a-8313-5c18759958a9	c0f9ff94-a28c-4e9c-a2c7-720aca11e966	2024-10-19 21:55:30.958892	\N	\N	\N	\N	f
b3117a33-6280-4bcd-981d-86a87705f58a	Deleniti ad et.	Nisi aut nobis quo similique quia quod iste et labore quam odio omnis porro qui architecto rem quia quam dolore et iure eos voluptatem hic et commodi voluptatem qui recusandae sunt dolores architecto dolore vel similique aliquid voluptatem ipsa ullam excepturi incidunt sed facere id qui rerum voluptatibus est qui.	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:30.958904	\N	\N	\N	\N	f
b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	Sunt dolores excepturi.	Labore facilis deleniti aperiam dolore eum qui nobis ipsam voluptate et vel inventore incidunt quidem et omnis quisquam quod ut et ut aut dolorem neque quos reiciendis eos non est aut reiciendis ut ipsum illum qui voluptate rerum beatae quasi quisquam id corrupti explicabo et quas dolorem sed est quos.	fadd55dc-c457-41a6-9723-c259182f0035	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 21:55:30.958928	\N	\N	\N	\N	f
bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	Dicta rerum itaque.	Nihil dolorem porro aut nam consequatur esse facilis eum voluptate culpa nihil et molestiae sit qui at libero voluptatem et ratione autem nobis voluptas mollitia eum qui consequuntur aut est sapiente hic quia adipisci blanditiis accusamus et officiis et quibusdam qui rerum provident autem dolorum quia minima inventore consequatur velit.	13ba9424-00b3-40a6-92c8-a9426207a2d9	d723eed5-78a1-4fab-9c9d-08efced4b861	2024-10-19 21:55:30.958492	\N	\N	\N	\N	f
c11966e2-ee59-4f31-9807-791e2e1c9a3d	Eum eligendi recusandae.	Nobis qui voluptas velit consectetur consequatur nobis neque veniam corporis dolores est beatae ut tenetur et maxime illum maxime ab possimus occaecati corporis consequatur qui sunt ut tenetur accusamus quia ratione sapiente et ex ab quas sint voluptatem quod odio in et totam alias doloremque est sed laborum dolor qui.	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:30.958547	\N	\N	\N	\N	f
c757d67e-b10d-49f4-b446-3010ae9f9591	Explicabo veritatis inventore.	Cumque aut distinctio harum officiis alias ut ea error suscipit maxime dolores aut praesentium aliquid assumenda et tempora voluptas rerum eligendi excepturi assumenda optio libero error at omnis dignissimos illum sint magnam unde non aut recusandae enim recusandae ut neque sapiente autem explicabo fugit quo saepe et est neque rerum.	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:30.958991	\N	\N	\N	\N	f
c92728b1-d0ee-4888-80e0-0f398b3c77db	Quaerat autem exercitationem.	Debitis laborum in aut perspiciatis corrupti enim dolor amet odio sapiente provident officia voluptatem esse eveniet dolor maiores quia voluptas et veritatis debitis maiores sit et aliquam quisquam dolorem praesentium totam facilis optio delectus est ut esse nemo possimus quia dolores deleniti quae autem nulla id quis magnam impedit et.	1faf9d72-1396-4e99-935d-547b226327c7	3054da29-a2e4-41b0-b7ac-9f3f4769e461	2024-10-19 21:55:30.958451	\N	\N	\N	\N	f
cef12cfc-5e1d-42c4-b7a0-90b57b0e2a36	Sit minus quam.	Maxime atque sed maiores quaerat dolores in molestiae tempora nihil autem nobis porro consequatur ea molestiae consequatur consequatur magni odio dolorem quaerat id veritatis aut voluptas dignissimos non dignissimos quia et accusamus doloremque similique ducimus quod fuga architecto veniam eaque voluptas sed a quidem provident odit ullam ut numquam aut.	53453386-8816-485f-9a08-22c07cf29d22	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 21:55:30.958608	\N	\N	\N	\N	f
d3935e8e-8abe-46dc-878a-4434da7af9ec	A rerum rerum.	Ad fugiat debitis dignissimos sapiente minima quasi atque nostrum minima aliquam numquam sunt autem aperiam adipisci quidem et tenetur sit earum perferendis ut sed quo error voluptas maxime quis omnis nihil pariatur ut eum aspernatur sit numquam omnis dolorem porro deleniti voluptate perferendis velit autem id rerum et nemo voluptas.	8b92673a-ba81-4629-aea9-41444a46a0dc	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 21:55:30.95885	\N	\N	\N	\N	f
d9baa37d-fd31-4cd7-81f3-bcb7902445b0	Sed qui consequuntur.	Alias consequatur et eligendi sed sit quia ut velit sed numquam earum eaque consequatur temporibus magni quia aperiam assumenda qui quam ut neque maiores nihil alias totam esse velit temporibus eos totam est distinctio nisi officia qui nostrum quos et aliquam sequi blanditiis perferendis ipsam consequatur repellendus ipsam ad corrupti.	0b996fe8-4582-412b-adfb-6fa402c25bf4	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:55:30.95888	\N	\N	\N	\N	f
deb52d19-077b-42d0-8949-2a7826f2c6a1	Vel accusantium sit.	A porro consequatur quas quod et et molestiae fugiat natus odit voluptates expedita sit ut impedit quisquam laudantium quas officia dicta et sed pariatur qui sunt asperiores tempora molestias adipisci distinctio laboriosam saepe sit sed rerum qui asperiores cumque et vitae quo soluta molestias ut repellendus corrupti impedit voluptatem accusamus.	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:55:30.958839	\N	\N	\N	\N	f
e33d90bd-73e3-4002-9da2-db6d1180de0e	Amet inventore sunt.	Repellendus modi dolor aperiam velit quae ipsa velit corrupti cum perferendis quasi repellendus atque vel sed sequi enim eum minus velit ad quia fugit aperiam dignissimos vitae aperiam id sit maiores id id inventore aspernatur sunt est eum laborum deserunt labore quae dolorem debitis laborum ut nisi nihil doloribus cum.	275ddc93-92b8-419a-ab96-baeb34d89c19	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 21:55:30.958711	\N	\N	\N	\N	f
eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	Odio et cumque.	Et dolorem assumenda laudantium unde velit sunt aut veniam mollitia recusandae aliquid officia laboriosam repellat quaerat neque et eveniet et reiciendis hic porro iure hic non voluptas eum officia consequatur sed aut delectus hic tempora veritatis corrupti hic ut amet qui harum repellendus occaecati molestias qui repudiandae doloremque vitae voluptates.	39ad1877-9e15-4498-88bb-ef6d617a23d2	7f003833-3d8a-4f3c-9c18-7986180847e4	2024-10-19 21:55:30.958785	\N	\N	\N	\N	f
eccb8dd8-849f-46c6-88e2-0d636544a8c4	Officia culpa sed.	Ducimus totam aut id labore quasi fugiat omnis asperiores numquam dolores maxime quis ipsum consequuntur dolorem consequatur velit quis voluptatum eaque accusantium veniam nostrum deleniti quod quae et voluptates cupiditate quis iure et distinctio alias odio ipsa alias veritatis quisquam alias et eum perspiciatis sit nisi dicta tenetur omnis et.	fe1e460d-16ac-4fcd-b512-2413b8cb3256	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 21:55:30.958866	\N	\N	\N	\N	f
f6b0f401-8c6e-4d62-a809-15d6006ee100	Sed expedita numquam.	Et quis et harum dolores aut doloribus aut consequatur dolorum minima occaecati sed eos minima est quidem et architecto laborum sequi possimus voluptatum fugit sint error nemo expedita qui qui nihil unde qui nobis est necessitatibus veniam autem et dolorem illo qui non sed commodi unde delectus dolorem in iusto.	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:55:30.9587	\N	\N	\N	\N	f
f8ff0b49-a5bd-42c2-a557-395bc9216a8a	Magnam voluptas consectetur.	Quod velit magnam consequatur libero rerum omnis officia vel est voluptatum asperiores aut aut incidunt suscipit nihil id architecto impedit nulla fugiat ut illo illo recusandae dolores voluptatem qui aut et asperiores et tenetur autem est maiores tempore possimus illum delectus velit est libero et provident sit officiis possimus voluptatibus.	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:30.958661	\N	\N	\N	\N	f
\.


--
-- TOC entry 2981 (class 0 OID 16621)
-- Dependencies: 205
-- Data for Name: groups_members; Type: TABLE DATA; Schema: public; Owner: infinitynetUser
--

COPY public.groups_members (id, role, group_id, user_profile_id, created_by, created_at, updated_by, updated_at, deleted_by, deleted_at, is_deleted) FROM stdin;
00476288-d35a-412b-9009-8d373c9c4d3f	0	d3935e8e-8abe-46dc-878a-4434da7af9ec	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:31.105381	\N	\N	\N	\N	f
00d79572-7cc8-445e-a9fa-adae16ca1cb0	0	ac641cb5-3883-4fbd-9783-9770175859f1	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:31.104119	\N	\N	\N	\N	f
01306850-7c3f-4787-b665-e43bd82c9d86	2	6b0a3307-50cd-4ab8-b239-52ec9227ff19	9f64a38d-8cdd-4a21-a529-9747a9331998	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 21:55:31.104312	\N	\N	\N	\N	f
01c77886-fb30-42ce-9aea-2a9a03b693cd	1	c92728b1-d0ee-4888-80e0-0f398b3c77db	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.103666	\N	\N	\N	\N	f
028c4f21-b3a8-4000-87e3-91d41923f83b	1	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	13ba9424-00b3-40a6-92c8-a9426207a2d9	d723eed5-78a1-4fab-9c9d-08efced4b861	2024-10-19 21:55:31.103701	\N	\N	\N	\N	f
030cbe67-0c5e-49e3-88fc-f0e2404cdb70	2	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	978e2b3f-9c26-41f0-b3c4-cba2e492767f	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:55:31.105842	\N	\N	\N	\N	f
0436307c-906e-444b-806b-c47594aaa616	2	6b0a3307-50cd-4ab8-b239-52ec9227ff19	6b8b0603-8e07-4181-92ec-ee13f0e768ce	41866800-c7ac-46ac-9cc8-a6190d3e47ce	2024-10-19 21:55:31.104963	\N	\N	\N	\N	f
07b2288a-4b08-489a-aebd-50bc68c8d5eb	2	38e635f2-1b68-473b-b941-1e81d253578f	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:31.106248	\N	\N	\N	\N	f
082264de-2f46-42ed-b011-f8c85bc70480	1	682c7de5-825d-4037-b630-0662381923b7	959b7d55-62bf-42c0-a313-33054551abb5	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 21:55:31.105029	\N	\N	\N	\N	f
09631a11-0c57-4f23-8378-00520a5d3736	1	40e92b5b-736d-4b63-ad95-7dfe2d26bc04	134e6153-f93b-4592-8bd7-ae30e9321017	dc2623fe-8a17-4340-abf9-d51a6e118efc	2024-10-19 21:55:31.106299	\N	\N	\N	\N	f
0a278db2-e063-47f1-9b41-bdcf348adb18	0	68151439-6890-4954-9559-06b02c84acdd	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.105344	\N	\N	\N	\N	f
0a4a240d-5480-42af-9039-6b7e70f7ee76	1	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	978e2b3f-9c26-41f0-b3c4-cba2e492767f	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:55:31.103115	\N	\N	\N	\N	f
0c01c61b-317d-4038-a1a4-deebdfe1fcf3	0	38e635f2-1b68-473b-b941-1e81d253578f	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 21:55:31.103019	\N	\N	\N	\N	f
0c55040b-bf1b-4c35-90ba-b140212182c5	1	32a4c31c-5048-43d8-b8e4-5734f0f6741c	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.104993	\N	\N	\N	\N	f
0e57e042-4958-4581-93bc-a23963232193	1	ac641cb5-3883-4fbd-9783-9770175859f1	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 21:55:31.106108	\N	\N	\N	\N	f
11b85181-dcd4-414e-9f7e-13c2c1af928f	2	26ab939c-c075-43f7-b16e-6a695866d173	e21d9b47-d1bb-4c02-9704-acff338cf963	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 21:55:31.103534	\N	\N	\N	\N	f
12b82ad5-5f06-4c8c-a38f-b093d4e267df	0	39796686-5676-4e48-914a-f405fbead580	ed964db3-afac-426e-8988-c2ed54a89510	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 21:55:31.104909	\N	\N	\N	\N	f
1457334c-7aa7-4733-b4ff-11c214b213e1	1	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:55:31.105147	\N	\N	\N	\N	f
1598be06-da23-47a5-b189-b7a89494bd76	2	12b52fea-b990-404f-9f77-13d66ec80399	9f64a38d-8cdd-4a21-a529-9747a9331998	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 21:55:31.10384	\N	\N	\N	\N	f
15ada026-562f-4557-a3a3-c834eb9f0db9	1	77bedc14-cd28-4fd7-8550-90cab9470ba4	d1372bba-be85-473c-8086-02a7c9890140	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:55:31.103763	\N	\N	\N	\N	f
15d697c1-8e12-4f4b-8535-d0160da8048d	2	6f51d38c-087b-4b4c-a69e-edb5b593a401	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:31.103631	\N	\N	\N	\N	f
168050d0-80ac-40cb-98f3-234f1891ac36	2	2edf5638-0175-4d58-81fc-92d37118727c	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.103865	\N	\N	\N	\N	f
16b297bb-70c8-4b24-8976-e7dd21630060	2	39796686-5676-4e48-914a-f405fbead580	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 21:55:31.102903	\N	\N	\N	\N	f
177d9843-d15d-4525-ae05-90661c4ae308	1	add17a7a-8cdd-47f2-ab30-e237b54ba751	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.104626	\N	\N	\N	\N	f
188a961d-0bc7-4848-8e78-489448ef37ec	1	39da44d5-76ab-47bc-9ff8-121c965e47d5	bb05cc9c-87a1-4d43-b253-d172e2117ff2	694020bc-a98b-4a12-93da-c9331c68619a	2024-10-19 21:55:31.102958	\N	\N	\N	\N	f
1a5cdf10-a884-4d2e-96ba-1b978f58f197	2	6f51d38c-087b-4b4c-a69e-edb5b593a401	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 21:55:31.105761	\N	\N	\N	\N	f
1a76bee6-210c-4c7f-a53d-6fca1986d803	1	e33d90bd-73e3-4002-9da2-db6d1180de0e	6c1fa607-dced-475d-9ad2-1e8ede9071d2	29198ed7-c2be-46cd-a0ed-36bd6a05efbf	2024-10-19 21:55:31.103484	\N	\N	\N	\N	f
1bcdcde7-4f8a-4dff-bc12-234e37c82028	2	01b040e9-784f-4edc-a439-2996df603eae	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:31.105955	\N	\N	\N	\N	f
1c5e7a3c-9340-4c78-8e3c-6160eb80e32e	2	68151439-6890-4954-9559-06b02c84acdd	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:31.105172	\N	\N	\N	\N	f
1ca8f085-e35f-4934-a6ad-32ba63cf0de7	1	cef12cfc-5e1d-42c4-b7a0-90b57b0e2a36	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:31.103473	\N	\N	\N	\N	f
1d0fa143-c3ae-411b-8c6a-83e47ba5cea3	0	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	612e214e-4fe6-4b17-b9af-8b8b26bf180e	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 21:55:31.104427	\N	\N	\N	\N	f
1f7ca52a-ccd8-4847-a0a7-c3dc1b81e0a4	0	054e6a0d-2b3b-4286-b4de-d02ed666ab83	5f55d75a-ca3a-4375-bdc4-afb591aef21d	aa223821-cdb2-4061-a4bc-55c2ef4f69d0	2024-10-19 21:55:31.10599	\N	\N	\N	\N	f
215eb3a2-3de2-4908-a318-394bdeefb592	2	deb52d19-077b-42d0-8949-2a7826f2c6a1	83c97377-4790-4e12-9b61-c0456fe642b2	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:55:31.104604	\N	\N	\N	\N	f
23212a84-404a-451d-8860-7a5b09d81cd0	0	e33d90bd-73e3-4002-9da2-db6d1180de0e	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:31.10631	\N	\N	\N	\N	f
252ff870-3789-4d15-850c-7fdf27a33213	1	12b52fea-b990-404f-9f77-13d66ec80399	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 21:55:31.10603	\N	\N	\N	\N	f
26718aaf-b38a-4731-bf3e-fb27c1be612d	2	a7c48de9-4698-47cf-ad56-cbaaede24885	22e64c46-97c3-40a7-a4aa-4b11eb838446	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 21:55:31.104651	\N	\N	\N	\N	f
28247dd8-03ea-4185-ba29-d8d21cf3c9ab	1	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	7b42cb26-668a-4037-8ffc-68058704a460	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 21:55:31.104348	\N	\N	\N	\N	f
28caeec2-10c5-4c09-93c3-53fa89a3223b	0	2edf5638-0175-4d58-81fc-92d37118727c	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.103326	\N	\N	\N	\N	f
2b18b571-7ad0-48ee-9180-071d1c181df5	0	27b1736a-9ead-4a1a-9156-fa8922470eef	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 21:55:31.103594	\N	\N	\N	\N	f
2c8bd3a3-88bc-40b5-8e00-c6ebc0ad3f98	0	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.104615	\N	\N	\N	\N	f
2cae65a5-40c2-4f19-a8ac-e6efd0b50980	2	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.105064	\N	\N	\N	\N	f
2f5c5d8f-4719-49db-becd-6b0e1d423ad5	0	d3935e8e-8abe-46dc-878a-4434da7af9ec	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 21:55:31.103341	\N	\N	\N	\N	f
302121b4-c685-40d7-99f9-3e6c5b3f4736	2	c92728b1-d0ee-4888-80e0-0f398b3c77db	14baebc0-0189-423c-a14c-d62ffe981f63	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 21:55:31.103569	\N	\N	\N	\N	f
3134ece8-f887-4324-9e98-c399e94fa22e	2	82e8961c-1ae8-4912-b4dc-51173b3fdfe6	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.105101	\N	\N	\N	\N	f
31692808-a29e-4431-8dc9-035a8a13321b	1	deb52d19-077b-42d0-8949-2a7826f2c6a1	7374bc88-8afb-4236-9fa0-d75adad253a0	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:55:31.104372	\N	\N	\N	\N	f
31efffdc-a206-4a56-8f7c-a52a99c492be	0	38e635f2-1b68-473b-b941-1e81d253578f	74d9ea46-5729-454f-b994-8faee093ddef	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 21:55:31.103716	\N	\N	\N	\N	f
328c5da2-534c-4047-8a90-0a047781768c	0	deb52d19-077b-42d0-8949-2a7826f2c6a1	84609dec-b050-496e-81be-301a1334919a	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:55:31.105651	\N	\N	\N	\N	f
32e59f9e-f1cd-426a-80b6-12b0fcbf8e66	0	77bedc14-cd28-4fd7-8550-90cab9470ba4	ed964db3-afac-426e-8988-c2ed54a89510	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 21:55:31.105602	\N	\N	\N	\N	f
342fb569-f7dd-4395-a8a6-af797847bb27	1	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	fe1e460d-16ac-4fcd-b512-2413b8cb3256	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 21:55:31.105233	\N	\N	\N	\N	f
35f1c743-af5b-41d5-9b0e-47415be9fa01	2	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	22e64c46-97c3-40a7-a4aa-4b11eb838446	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 21:55:31.103853	\N	\N	\N	\N	f
367e2a9b-a999-46a1-8466-165a25f727cb	2	26ab939c-c075-43f7-b16e-6a695866d173	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.104562	\N	\N	\N	\N	f
36d6c24f-b44d-4a37-9b9d-ab117ed6efb3	0	295c035f-0873-479b-b155-6746e039e598	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:31.103512	\N	\N	\N	\N	f
36d759b4-ba2f-4c0d-bb47-029dcdcd8e6d	1	a7c48de9-4698-47cf-ad56-cbaaede24885	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 21:55:31.103876	\N	\N	\N	\N	f
36e6faaf-3c80-496d-a54b-5be03b0519ea	2	4dfeb3d3-2c9b-497e-8384-55e94215571d	b0d1d45b-c71b-4578-8ac0-01c30b49131b	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 21:55:31.102764	\N	\N	\N	\N	f
377ec994-b055-438b-ac25-ca22aae430f7	2	a5c74b1d-9279-441a-a283-8a9a67e6378c	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:31.105929	\N	\N	\N	\N	f
38363c68-2546-4b38-a6d3-1bcb8b50b0c2	1	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 21:55:31.103094	\N	\N	\N	\N	f
386d0fc1-7b6a-4323-843c-2672ca112c4b	1	27b1736a-9ead-4a1a-9156-fa8922470eef	53453386-8816-485f-9a08-22c07cf29d22	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 21:55:31.106211	\N	\N	\N	\N	f
387d6fce-e3ba-4a66-9991-048709dbae2d	0	a5c74b1d-9279-441a-a283-8a9a67e6378c	eba19f8f-0936-45eb-88bc-9c83772a1d93	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 21:55:31.103727	\N	\N	\N	\N	f
38bd9c21-dfcd-4ac6-8dfe-22f5a8abbf21	2	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	c6d25490-d32a-410d-be77-5370cc1482a2	d776b5c6-c7ca-4d5a-9fd3-6d2828447425	2024-10-19 21:55:31.10533	\N	\N	\N	\N	f
38faafbe-65e9-4d59-a1e6-1f38bd9575fd	1	295c035f-0873-479b-b155-6746e039e598	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 21:55:31.105392	\N	\N	\N	\N	f
39e630e4-60ea-4320-8d01-9a3674788872	2	bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	b1469423-4113-490e-bcd6-b79a146c3f81	0ecdbfd7-a759-41de-81db-f550960f3f10	2024-10-19 21:55:31.10436	\N	\N	\N	\N	f
3a3e05c4-3474-4732-affe-1b8a7a582708	1	095f9791-d050-4330-906b-0647ea1786f4	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.102969	\N	\N	\N	\N	f
3b8341f1-9bd0-4626-8940-2fdf2dd62e91	2	eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:31.104573	\N	\N	\N	\N	f
3c78ab79-1106-44b4-bab9-5cd10539dcc3	2	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	fa846317-fe54-4f52-b8d6-6a618387a5da	b56dfb50-cf66-498e-80b8-61876a9c4d92	2024-10-19 21:55:31.103256	\N	\N	\N	\N	f
3c8867b0-bfbd-4376-856c-e683a64887e0	2	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 21:55:31.102928	\N	\N	\N	\N	f
3d66b5a2-88aa-4b28-b567-831ebce8cc54	0	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.104741	\N	\N	\N	\N	f
40b1856a-77cf-4f12-a8f1-b440af450d8f	1	40e92b5b-736d-4b63-ad95-7dfe2d26bc04	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.105366	\N	\N	\N	\N	f
43cec5cc-adff-4d12-a1c4-8ad0901e1a26	2	cef12cfc-5e1d-42c4-b7a0-90b57b0e2a36	705391da-77b5-4f08-b176-301a5f1bbc0d	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 21:55:31.104171	\N	\N	\N	\N	f
44039470-e766-4256-ac1a-619f03648d57	2	5310a3e6-5a16-4090-a632-105cae7d42eb	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:31.105295	\N	\N	\N	\N	f
444ef23a-d727-4ea8-aaaf-dd73f183d0e1	2	c92728b1-d0ee-4888-80e0-0f398b3c77db	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.103214	\N	\N	\N	\N	f
44760dcd-2c66-4383-aa8e-1dbe29e69cd9	1	2edf5638-0175-4d58-81fc-92d37118727c	53453386-8816-485f-9a08-22c07cf29d22	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 21:55:31.104936	\N	\N	\N	\N	f
467b2f05-217b-4691-a997-c7d22e389abd	1	39da44d5-76ab-47bc-9ff8-121c965e47d5	439c9800-35c9-48ee-8549-9c293a107743	da569c42-3e83-47d7-9205-a23c3e1e34f3	2024-10-19 21:55:31.104323	\N	\N	\N	\N	f
46b0bad7-e68c-499e-ba0d-2ff3a1120022	2	add17a7a-8cdd-47f2-ab30-e237b54ba751	84609dec-b050-496e-81be-301a1334919a	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:55:31.103582	\N	\N	\N	\N	f
47aa9db4-4644-403d-9c8c-3a1c623254da	2	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	275ddc93-92b8-419a-ab96-baeb34d89c19	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 21:55:31.104183	\N	\N	\N	\N	f
48188e33-fde1-48ec-bbaa-5212c8cfebcb	0	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	2e6b7127-5e54-43eb-a21f-64c57143824d	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 21:55:31.104094	\N	\N	\N	\N	f
48cbb768-f8a3-4575-95c6-c7ca79849725	2	26ab939c-c075-43f7-b16e-6a695866d173	6700632c-6c3b-4d7e-81dd-8b2151b60502	6d48e156-8327-48d6-91d9-61ce20e3125b	2024-10-19 21:55:31.105869	\N	\N	\N	\N	f
490d74e9-23ca-482b-a1f2-f46f263294ae	2	6f51d38c-087b-4b4c-a69e-edb5b593a401	14baebc0-0189-423c-a14c-d62ffe981f63	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 21:55:31.105567	\N	\N	\N	\N	f
4963598a-1edd-4e27-a010-4b511b1e01cc	0	e33d90bd-73e3-4002-9da2-db6d1180de0e	612e214e-4fe6-4b17-b9af-8b8b26bf180e	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 21:55:31.105686	\N	\N	\N	\N	f
4a81928e-b361-4f93-8673-7a37589c64cb	2	5310a3e6-5a16-4090-a632-105cae7d42eb	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.104886	\N	\N	\N	\N	f
4b00eeca-cdb5-424d-ac42-646b374f688e	1	12b52fea-b990-404f-9f77-13d66ec80399	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 21:55:31.106019	\N	\N	\N	\N	f
4ba41cfd-d116-4144-86db-8cb7a3ac0374	1	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	fadd55dc-c457-41a6-9723-c259182f0035	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 21:55:31.106284	\N	\N	\N	\N	f
4ba848cc-a0f3-48ea-8afb-01e15b53f434	1	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.104134	\N	\N	\N	\N	f
4e0e90d8-cfce-4cc9-bd4a-4e982c93ff63	0	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	92d0bb28-a2c9-476f-878f-c6ebe2f04e2c	f8c48d9c-5c29-4f8a-8b87-5c79a77b4bce	2024-10-19 21:55:31.104334	\N	\N	\N	\N	f
4e612b69-b2ca-4027-ba4a-57c3eb889165	1	27b1736a-9ead-4a1a-9156-fa8922470eef	39ad1877-9e15-4498-88bb-ef6d617a23d2	7f003833-3d8a-4f3c-9c18-7986180847e4	2024-10-19 21:55:31.106164	\N	\N	\N	\N	f
4ee8dcb5-a3bc-47c7-a1cb-358425cdafe7	2	e33d90bd-73e3-4002-9da2-db6d1180de0e	134e6153-f93b-4592-8bd7-ae30e9321017	dc2623fe-8a17-4340-abf9-d51a6e118efc	2024-10-19 21:55:31.105917	\N	\N	\N	\N	f
4efe421d-ef7f-4dbd-8990-c044a05bd076	1	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.103269	\N	\N	\N	\N	f
4fabbb7c-997a-4d8e-a6de-2f969444b95f	0	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:55:31.10392	\N	\N	\N	\N	f
500284da-185b-4b9a-ad6e-34978dd2bac1	1	d3935e8e-8abe-46dc-878a-4434da7af9ec	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.102864	\N	\N	\N	\N	f
50b77fba-404e-4fd8-a3ac-96dc5985a20b	1	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	c6d25490-d32a-410d-be77-5370cc1482a2	d776b5c6-c7ca-4d5a-9fd3-6d2828447425	2024-10-19 21:55:31.103751	\N	\N	\N	\N	f
51321f1c-f2e8-4c08-91fc-0ed3272ff193	0	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:31.105673	\N	\N	\N	\N	f
51b71a93-46a7-4746-93b6-edb98c0f3f4c	1	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	30d72372-2aee-46cd-ab7f-56dcaefe600c	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 21:55:31.103376	\N	\N	\N	\N	f
533bb45d-6e98-44ba-9b1d-fd6140743fb5	2	c92728b1-d0ee-4888-80e0-0f398b3c77db	134e6153-f93b-4592-8bd7-ae30e9321017	dc2623fe-8a17-4340-abf9-d51a6e118efc	2024-10-19 21:55:31.104438	\N	\N	\N	\N	f
537cbe7e-a70d-4732-827b-f56cda75bffe	1	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	9f64a38d-8cdd-4a21-a529-9747a9331998	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 21:55:31.103558	\N	\N	\N	\N	f
5447f6ee-8049-4eb7-bad2-da6a9eafad6e	2	295c035f-0873-479b-b155-6746e039e598	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	15d219ed-b4eb-46de-9f55-741dd7dcec62	2024-10-19 21:55:31.10362	\N	\N	\N	\N	f
55057370-bb99-4d02-b62b-d91dcc6b1b27	2	5310a3e6-5a16-4090-a632-105cae7d42eb	2eb2ae7e-b05a-45c8-83ef-a23717e17947	bcb42de0-64c2-4e11-890b-7b3de06d0924	2024-10-19 21:55:31.104236	\N	\N	\N	\N	f
581daffc-11e8-4b10-b8f6-90a027366e7b	2	4dfeb3d3-2c9b-497e-8384-55e94215571d	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:31.104679	\N	\N	\N	\N	f
58e38c23-1617-44ca-acb9-dd0ebc6c12b5	0	f6b0f401-8c6e-4d62-a809-15d6006ee100	e095bbae-68d2-4077-9036-697c526736d4	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:55:31.104584	\N	\N	\N	\N	f
5a4687e9-b141-4f18-9216-be90775823f1	0	f6b0f401-8c6e-4d62-a809-15d6006ee100	2e6b7127-5e54-43eb-a21f-64c57143824d	26261306-88f5-4e8c-92fa-d96a825768d2	2024-10-19 21:55:31.105783	\N	\N	\N	\N	f
5ae0f88a-5f36-4377-a62a-214bbb731361	2	27b1736a-9ead-4a1a-9156-fa8922470eef	ed964db3-afac-426e-8988-c2ed54a89510	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 21:55:31.104836	\N	\N	\N	\N	f
5cbbbe7b-d85b-456a-9884-3e0de0e9643e	1	682c7de5-825d-4037-b630-0662381923b7	4929722e-df51-411e-8c00-59955f7d8fd8	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 21:55:31.104789	\N	\N	\N	\N	f
5d85cc4c-310f-4d51-95bd-fb8d89062ccc	1	add17a7a-8cdd-47f2-ab30-e237b54ba751	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 21:55:31.103178	\N	\N	\N	\N	f
5db094fb-e403-49b5-824b-3539d9d8980e	2	054e6a0d-2b3b-4286-b4de-d02ed666ab83	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.103006	\N	\N	\N	\N	f
5db8cb7e-8a94-4944-baf4-6021870e5a2d	0	71691b66-37b0-4a42-95e2-f6d2c14a7d75	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 21:55:31.104662	\N	\N	\N	\N	f
606afc17-d03c-421f-b053-0b090647991e	2	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	f8ebf6a1-45d5-4b39-a5fa-4c862867ee36	2024-10-19 21:55:31.104273	\N	\N	\N	\N	f
60ac9ee4-ecbd-48b9-9ca3-6bc9fc878619	0	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	9612f20e-6fce-4190-bc29-b31d7d3d9188	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:55:31.10346	\N	\N	\N	\N	f
61238911-afde-4e07-8f85-9aba6ec25e4d	2	68151439-6890-4954-9559-06b02c84acdd	45370c44-1d4d-4834-8cd5-3551b5d30199	d34efe03-6baf-42df-8e7b-0418ac94c7f8	2024-10-19 21:55:31.104549	\N	\N	\N	\N	f
62d602d3-02be-4047-825a-fdf9a9f7dc82	0	eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	18e845d8-400b-4d12-a414-9cd440f92677	ecfbb998-043f-40d3-af8c-c0a0cf04f57b	2024-10-19 21:55:31.105697	\N	\N	\N	\N	f
646a6c83-ee50-4764-9cf0-41f98f6c16eb	1	27b1736a-9ead-4a1a-9156-fa8922470eef	1bc4061b-cefd-44dc-89e8-57d1c4ad078a	aa61d4be-936a-46ea-8176-83e0c09fb5cf	2024-10-19 21:55:31.102549	\N	\N	\N	\N	f
64737827-54e6-436d-a22d-50ad7715ad52	0	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	9612f20e-6fce-4190-bc29-b31d7d3d9188	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:55:31.106148	\N	\N	\N	\N	f
64c61f95-7e5c-4a31-b6f8-0f1a9cb4e436	1	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	14a6b1d0-f886-4f46-9166-a134668d16ab	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 21:55:31.103896	\N	\N	\N	\N	f
64e6d0f2-17cb-4665-9bcb-446fad816c1d	2	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	78532cb2-f350-4c98-bce2-e94afd8369c6	4bbe97ff-9028-4030-967e-34d7eae8f332	2024-10-19 21:55:31.103153	\N	\N	\N	\N	f
65e18329-27f3-42e6-b32f-2cbe5fbf0d97	0	27b1736a-9ead-4a1a-9156-fa8922470eef	14a6b1d0-f886-4f46-9166-a134668d16ab	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 21:55:31.103388	\N	\N	\N	\N	f
68403d28-68bb-4a74-89cd-9514f6a46280	2	2e68a27c-4347-4b83-91a5-a44e7c47473c	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.106083	\N	\N	\N	\N	f
696f2688-3fe0-4731-b33c-0d520e9cdccb	0	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.105966	\N	\N	\N	\N	f
69b1d672-26cc-4351-8c4a-5931b0b4ba85	2	26ab939c-c075-43f7-b16e-6a695866d173	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 21:55:31.10581	\N	\N	\N	\N	f
69d7fff1-148a-419e-aab8-b907ef3912d8	0	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.104947	\N	\N	\N	\N	f
6d083d33-5f84-41fd-8786-22fd191f690c	0	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	275ddc93-92b8-419a-ab96-baeb34d89c19	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 21:55:31.104777	\N	\N	\N	\N	f
6d96dc3c-e396-4e98-9ded-12ed94ea69f1	1	a5c74b1d-9279-441a-a283-8a9a67e6378c	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.103934	\N	\N	\N	\N	f
6dd54432-d9a6-4385-8217-9fe9de4852fa	1	2e68a27c-4347-4b83-91a5-a44e7c47473c	49fa1298-7d26-4de1-b197-3005c3d03c0e	88ea9d8d-9bf0-40ed-a794-32835eac461a	2024-10-19 21:55:31.104859	\N	\N	\N	\N	f
6eba50e1-0cd7-430c-989d-46eb806ceb7f	0	ac641cb5-3883-4fbd-9783-9770175859f1	84609dec-b050-496e-81be-301a1334919a	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:55:31.10516	\N	\N	\N	\N	f
71bd3d45-af4d-4dda-a8cf-c0a93ece3d4d	0	295c035f-0873-479b-b155-6746e039e598	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 21:55:31.105734	\N	\N	\N	\N	f
721713dc-579f-447c-bf4c-61c2e4fcb51c	0	682c7de5-825d-4037-b630-0662381923b7	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.105123	\N	\N	\N	\N	f
74a655d5-f3c5-4a49-ba5a-b68cc7c45116	2	38e635f2-1b68-473b-b941-1e81d253578f	959b7d55-62bf-42c0-a313-33054551abb5	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 21:55:31.104224	\N	\N	\N	\N	f
758cea0b-c54a-4a98-8f58-636e67a13224	2	095f9791-d050-4330-906b-0647ea1786f4	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.104639	\N	\N	\N	\N	f
774156c0-b490-4ba0-bf27-a32de3392de5	2	2edf5638-0175-4d58-81fc-92d37118727c	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:31.105416	\N	\N	\N	\N	f
77fdf661-813e-4757-bbb6-a82573f15bad	0	39da44d5-76ab-47bc-9ff8-121c965e47d5	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 21:55:31.103352	\N	\N	\N	\N	f
79365665-8704-4d37-905f-63197efac453	0	71691b66-37b0-4a42-95e2-f6d2c14a7d75	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:31.105892	\N	\N	\N	\N	f
794bd083-db78-4267-9c34-e18a805768d3	0	054e6a0d-2b3b-4286-b4de-d02ed666ab83	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.10522	\N	\N	\N	\N	f
7ad3ca73-30ec-498a-8f97-a152381293af	1	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	b55f5bbd-4b95-448a-b38b-a1429002854b	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:55:31.104286	\N	\N	\N	\N	f
7b3e2c61-65c0-4722-9666-2fa1e00238cf	1	b3117a33-6280-4bcd-981d-86a87705f58a	74d9ea46-5729-454f-b994-8faee093ddef	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 21:55:31.103245	\N	\N	\N	\N	f
7c259b8d-d935-4e87-9575-589c88ad8b37	0	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 21:55:31.105453	\N	\N	\N	\N	f
7c68d4cb-9649-425f-ada8-37e25e97c3f4	0	39da44d5-76ab-47bc-9ff8-121c965e47d5	c2325fbe-7f7b-4d92-b73d-48d26e0c5047	8242c55f-d333-4a17-b709-18e5bc2cecc2	2024-10-19 21:55:31.102841	\N	\N	\N	\N	f
819a9e01-826e-46a1-8e5d-ed4a41146531	0	eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	fadd55dc-c457-41a6-9723-c259182f0035	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 21:55:31.106002	\N	\N	\N	\N	f
84a45897-4567-4ea8-8eb9-17ec55bb0856	1	b3117a33-6280-4bcd-981d-86a87705f58a	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 21:55:31.105578	\N	\N	\N	\N	f
85516b16-47be-4ff2-9a16-548a44e26d68	0	ac641cb5-3883-4fbd-9783-9770175859f1	b116c61a-f11d-46dc-b3dc-b66678e9fbb6	15d219ed-b4eb-46de-9f55-741dd7dcec62	2024-10-19 21:55:31.102879	\N	\N	\N	\N	f
864a0f82-afcc-47dc-813d-e40c11cc65b5	2	ac641cb5-3883-4fbd-9783-9770175859f1	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.103737	\N	\N	\N	\N	f
86fa30a6-25f1-47ea-a92b-f20c0e46f04e	1	2e68a27c-4347-4b83-91a5-a44e7c47473c	fadd55dc-c457-41a6-9723-c259182f0035	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 21:55:31.105662	\N	\N	\N	\N	f
87367efb-3d6f-4354-a3e0-c1aa041d25a3	0	c11966e2-ee59-4f31-9807-791e2e1c9a3d	ed964db3-afac-426e-8988-c2ed54a89510	6319f404-3c93-4b0c-8671-411ad83c16df	2024-10-19 21:55:31.106336	\N	\N	\N	\N	f
87e2cec9-f6b8-4c19-8dd3-83c79f6f5728	2	38e635f2-1b68-473b-b941-1e81d253578f	384d21de-6a0f-4c92-b0ef-540ff97079bc	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 21:55:31.105708	\N	\N	\N	\N	f
8bc3536d-e4ab-4af1-8827-77f2495ac785	1	4dfeb3d3-2c9b-497e-8384-55e94215571d	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.106187	\N	\N	\N	\N	f
8c40077a-1ae3-4e46-9f05-eef7d730a1bc	1	295c035f-0873-479b-b155-6746e039e598	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 21:55:31.105319	\N	\N	\N	\N	f
8c7c7dd7-e2b0-45af-b815-ce994547b1ea	1	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	384d21de-6a0f-4c92-b0ef-540ff97079bc	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 21:55:31.104825	\N	\N	\N	\N	f
8e0f193b-2a28-495d-8ec0-da903d35fa4e	1	71691b66-37b0-4a42-95e2-f6d2c14a7d75	1cc85c40-c092-4bee-adeb-3dc17e304563	3d3cb675-d596-49aa-89af-61479d8c8e8d	2024-10-19 21:55:31.103495	\N	\N	\N	\N	f
91e83b41-c804-41fd-8513-077450d730f2	2	f6b0f401-8c6e-4d62-a809-15d6006ee100	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:31.105822	\N	\N	\N	\N	f
9270c5be-0ea4-435f-8f2f-1a3e1369319e	2	e33d90bd-73e3-4002-9da2-db6d1180de0e	eba19f8f-0936-45eb-88bc-9c83772a1d93	8c5bf892-39e3-4369-b889-a828b8278ddc	2024-10-19 21:55:31.106057	\N	\N	\N	\N	f
92d481ff-1be4-48ce-af10-7ae8c874b7a8	1	054e6a0d-2b3b-4286-b4de-d02ed666ab83	2fa772f8-0fa4-472b-a154-14cf794d50e2	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 21:55:31.105463	\N	\N	\N	\N	f
93f4c278-8964-4509-a3c5-fad5f9db9e6d	2	f6b0f401-8c6e-4d62-a809-15d6006ee100	6e132241-d674-4195-b8c5-b6b4d320e3ce	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 21:55:31.103644	\N	\N	\N	\N	f
94aa5702-2d3b-4d86-89e2-2c34a5a28844	2	c92728b1-d0ee-4888-80e0-0f398b3c77db	be26aee1-0512-4e6a-8313-5c18759958a9	c0f9ff94-a28c-4e9c-a2c7-720aca11e966	2024-10-19 21:55:31.104039	\N	\N	\N	\N	f
958441dc-7e1d-4fbf-bc28-46265dad54c1	0	2e68a27c-4347-4b83-91a5-a44e7c47473c	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:31.105501	\N	\N	\N	\N	f
959ad9a5-ab07-4566-a9d1-aa024a4977b9	0	12b52fea-b990-404f-9f77-13d66ec80399	fa846317-fe54-4f52-b8d6-6a618387a5da	b56dfb50-cf66-498e-80b8-61876a9c4d92	2024-10-19 21:55:31.104521	\N	\N	\N	\N	f
969458ec-c2ed-445a-b948-df158a387603	1	6f51d38c-087b-4b4c-a69e-edb5b593a401	8f722abd-0123-4494-b71c-a21943484a3c	afee2031-2add-4c5a-b960-f79ac7a80490	2024-10-19 21:55:31.105197	\N	\N	\N	\N	f
96da24bb-21c7-4a8a-adb7-a0256dd5ac5d	2	12b52fea-b990-404f-9f77-13d66ec80399	612e214e-4fe6-4b17-b9af-8b8b26bf180e	5960c661-acbe-40ae-8911-9ca1c668bb02	2024-10-19 21:55:31.105772	\N	\N	\N	\N	f
97049b1d-7f3d-4e3b-8b14-9192860cc0e1	0	01b040e9-784f-4edc-a439-2996df603eae	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.103413	\N	\N	\N	\N	f
97febd24-e6c4-477f-ba43-060bd091f922	1	c757d67e-b10d-49f4-b446-3010ae9f9591	e095bbae-68d2-4077-9036-697c526736d4	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:55:31.10397	\N	\N	\N	\N	f
9882d68a-b116-4c44-a87b-78a6bd520e5a	2	bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	143437a3-503e-4e95-911d-4c6571ddea8e	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 21:55:31.105308	\N	\N	\N	\N	f
9892ab3e-159a-4f86-9798-ee7193033a69	1	a5c74b1d-9279-441a-a283-8a9a67e6378c	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.105183	\N	\N	\N	\N	f
98e17dfa-fd2e-411c-9f23-b828ef388b8c	0	12b52fea-b990-404f-9f77-13d66ec80399	9ca9bcee-c97f-4778-83f4-57fff49759d1	8febcf10-4332-4750-b0e8-3c64c7d204ad	2024-10-19 21:55:31.104499	\N	\N	\N	\N	f
98f4e7ad-063a-422b-8e86-4578b4a18a29	0	eccb8dd8-849f-46c6-88e2-0d636544a8c4	b6d54f8d-b08c-4f88-9db9-00008875256f	120acdc1-8799-412b-8fc8-67addf841f25	2024-10-19 21:55:31.103398	\N	\N	\N	\N	f
99037e54-fda6-43ad-a08f-1cc6dc96bc06	1	bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	8b92673a-ba81-4629-aea9-41444a46a0dc	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 21:55:31.102791	\N	\N	\N	\N	f
99633e1b-8635-4ae8-812c-45a0bb65fc5d	0	f6b0f401-8c6e-4d62-a809-15d6006ee100	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 21:55:31.105074	\N	\N	\N	\N	f
9a62e99d-d221-4521-a2d7-b368d439cbcd	0	6f51d38c-087b-4b4c-a69e-edb5b593a401	35d0da5e-7492-46d3-aaca-17455a353de9	80c16f07-671b-472d-be58-e5fd82bedce0	2024-10-19 21:55:31.105085	\N	\N	\N	\N	f
9acafe94-fa0a-4540-bf7a-50025907f8a6	2	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	20105f5a-82e0-4763-b12c-7a5ddc9abf83	d69d03da-d18a-4556-838f-0c9c4d81656d	2024-10-19 21:55:31.106236	\N	\N	\N	\N	f
9b2f9a71-0544-492e-94bd-c47ab3830da7	2	295c035f-0873-479b-b155-6746e039e598	b55f5bbd-4b95-448a-b38b-a1429002854b	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:55:31.106069	\N	\N	\N	\N	f
9b9aaf09-d47e-488c-b414-675c17d660e3	0	2edf5638-0175-4d58-81fc-92d37118727c	14a6b1d0-f886-4f46-9166-a134668d16ab	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 21:55:31.104065	\N	\N	\N	\N	f
9d062c84-4eb7-4453-87ac-d4c2060503fd	2	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.105979	\N	\N	\N	\N	f
9e31b3df-c282-4714-bfe6-1ad1e3bfb333	2	39796686-5676-4e48-914a-f405fbead580	fadd55dc-c457-41a6-9723-c259182f0035	365bf22b-e9ec-49b2-a509-ce91ecb12a36	2024-10-19 21:55:31.105615	\N	\N	\N	\N	f
9e85e8ae-aa9d-4073-889f-2fc0dfe45721	1	5310a3e6-5a16-4090-a632-105cae7d42eb	959b7d55-62bf-42c0-a313-33054551abb5	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 21:55:31.102995	\N	\N	\N	\N	f
9ef79758-4934-4f1b-af4c-709a355ddf37	2	39796686-5676-4e48-914a-f405fbead580	7374bc88-8afb-4236-9fa0-d75adad253a0	cc5755e6-f51c-45d4-b183-9821a5f92cc3	2024-10-19 21:55:31.10308	\N	\N	\N	\N	f
9f024388-12dc-4a66-8a7e-47c5a3500a56	2	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	09f405ed-f0c6-422c-847f-0e24f7c74aef	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 21:55:31.102852	\N	\N	\N	\N	f
9f382be2-5e03-4418-ae1e-341a761bc8dc	2	01b040e9-784f-4edc-a439-2996df603eae	b6d54f8d-b08c-4f88-9db9-00008875256f	120acdc1-8799-412b-8fc8-67addf841f25	2024-10-19 21:55:31.106273	\N	\N	\N	\N	f
9fcc55da-3c92-4838-ab2a-f7b7223df48f	1	c11966e2-ee59-4f31-9807-791e2e1c9a3d	cf6228c4-4f7d-45f4-a49d-3b5e9cd85c64	0fbc3ba7-9a40-486d-8f7f-def74004317c	2024-10-19 21:55:31.104475	\N	\N	\N	\N	f
a047e765-e383-4166-9636-c8199e9aab14	2	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	30d72372-2aee-46cd-ab7f-56dcaefe600c	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 21:55:31.103315	\N	\N	\N	\N	f
a0609732-4df7-468d-9744-4055abcb58f2	0	eccb8dd8-849f-46c6-88e2-0d636544a8c4	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:55:31.105854	\N	\N	\N	\N	f
a1b40a03-c08e-4fa0-af3c-b75111f60374	2	a5c74b1d-9279-441a-a283-8a9a67e6378c	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.105745	\N	\N	\N	\N	f
a1c8ec88-9ebb-4c91-ad7b-47619ab242a8	0	c11966e2-ee59-4f31-9807-791e2e1c9a3d	28ffe509-f3c0-4d56-aeb4-8668f16da5d5	e7d2a4ad-4c9c-4900-89f8-6bbcdadd81ea	2024-10-19 21:55:31.103815	\N	\N	\N	\N	f
a1d182c3-8890-4d0d-99f2-88b1e78542b3	1	2edf5638-0175-4d58-81fc-92d37118727c	d1372bba-be85-473c-8086-02a7c9890140	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:55:31.103957	\N	\N	\N	\N	f
a2bc14f0-d151-4318-81e0-573d8b3676a6	2	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	09f405ed-f0c6-422c-847f-0e24f7c74aef	b7fea93b-b368-4525-8fa3-cc0559c2447f	2024-10-19 21:55:31.104754	\N	\N	\N	\N	f
aa8a1124-eba7-4a96-8304-fafa428d180d	1	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	cd45e23d-ecee-4c2b-95f6-ee357e5b8c9b	c54da6cd-1221-4147-ab17-0cd309e389e0	2024-10-19 21:55:31.102916	\N	\N	\N	\N	f
aaf17c4f-f2be-467c-9bba-1b8122004495	1	d3935e8e-8abe-46dc-878a-4434da7af9ec	30d72372-2aee-46cd-ab7f-56dcaefe600c	9a6498c9-2787-4e17-851f-065ab6f9bc66	2024-10-19 21:55:31.104107	\N	\N	\N	\N	f
ac4c1c82-5680-4f1c-8634-0e13d543be57	0	682c7de5-825d-4037-b630-0662381923b7	6700632c-6c3b-4d7e-81dd-8b2151b60502	6d48e156-8327-48d6-91d9-61ce20e3125b	2024-10-19 21:55:31.105427	\N	\N	\N	\N	f
ad80cae5-3a88-4811-87c9-a49f7f1069c7	1	ac641cb5-3883-4fbd-9783-9770175859f1	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	f8ebf6a1-45d5-4b39-a5fa-4c862867ee36	2024-10-19 21:55:31.10588	\N	\N	\N	\N	f
aeceafe0-a414-42a0-a950-4edb8affe369	0	39796686-5676-4e48-914a-f405fbead580	18e845d8-400b-4d12-a414-9cd440f92677	ecfbb998-043f-40d3-af8c-c0a0cf04f57b	2024-10-19 21:55:31.103828	\N	\N	\N	\N	f
af26a075-9b4c-4f99-afbb-4480fd879980	2	2e68a27c-4347-4b83-91a5-a44e7c47473c	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 21:55:31.104694	\N	\N	\N	\N	f
afa98064-9414-4e95-a0e4-dbe797debafe	1	295c035f-0873-479b-b155-6746e039e598	0b996fe8-4582-412b-adfb-6fa402c25bf4	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:55:31.105528	\N	\N	\N	\N	f
b3bfbf8b-9f6d-4a4b-a114-a02cece7d5cb	1	d3935e8e-8abe-46dc-878a-4434da7af9ec	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.103189	\N	\N	\N	\N	f
b4f30981-a836-444b-a8b0-a4209d0b37f6	2	27b1736a-9ead-4a1a-9156-fa8922470eef	fe1e460d-16ac-4fcd-b512-2413b8cb3256	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 21:55:31.105637	\N	\N	\N	\N	f
b68e3ab5-dd55-4808-8853-461bdbb420cd	2	eccb8dd8-849f-46c6-88e2-0d636544a8c4	53453386-8816-485f-9a08-22c07cf29d22	9df8f4f1-1e5a-456d-8819-9584ff75446f	2024-10-19 21:55:31.104898	\N	\N	\N	\N	f
b6e71aca-1748-479f-8027-5d3a349fedb2	2	c757d67e-b10d-49f4-b446-3010ae9f9591	8b92673a-ba81-4629-aea9-41444a46a0dc	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 21:55:31.104411	\N	\N	\N	\N	f
b72d14d7-9e0b-4f99-9614-73d8cfb13bbb	0	6f51d38c-087b-4b4c-a69e-edb5b593a401	2fa772f8-0fa4-472b-a154-14cf794d50e2	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 21:55:31.103104	\N	\N	\N	\N	f
b7f7c8ec-78a7-40e7-8723-da378e98d238	1	095f9791-d050-4330-906b-0647ea1786f4	d0e23fb9-4596-463e-8578-c9acdcdb4c5f	97a977e7-ef81-4b31-9bd2-bd3c065dd17c	2024-10-19 21:55:31.105906	\N	\N	\N	\N	f
b844443b-b42b-4713-b4f5-5c449c4906ec	0	b3117a33-6280-4bcd-981d-86a87705f58a	b1469423-4113-490e-bcd6-b79a146c3f81	0ecdbfd7-a759-41de-81db-f550960f3f10	2024-10-19 21:55:31.102826	\N	\N	\N	\N	f
b9babdb6-68b9-4ad5-8dc0-cd11d3d49010	1	c92728b1-d0ee-4888-80e0-0f398b3c77db	27cf8d25-e68b-41e4-a2d2-245d2e9370e3	2f70e59a-3802-4f5c-a6ef-25b2cdde4f33	2024-10-19 21:55:31.103141	\N	\N	\N	\N	f
bb394339-82e5-46b0-a474-7375da9cee6f	2	deb52d19-077b-42d0-8949-2a7826f2c6a1	58071bf0-1435-4f5f-a67c-f7f1ca0a74dc	6af636c4-96e4-4f9e-96a0-794dc6541dc3	2024-10-19 21:55:31.106261	\N	\N	\N	\N	f
bca3c04d-aa25-4edd-8ff4-f848ecb4fd9f	0	5310a3e6-5a16-4090-a632-105cae7d42eb	3d8be820-f83f-4579-b8e2-a8c4b5465d69	67bd2b8c-552a-4227-ab05-604f8f62a655	2024-10-19 21:55:31.105051	\N	\N	\N	\N	f
bcb0eaf7-f4ba-433a-b947-fe3b23d3ac21	0	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 21:55:31.103234	\N	\N	\N	\N	f
bd6692e1-5812-460b-9f0b-8f476ef9894f	1	c92728b1-d0ee-4888-80e0-0f398b3c77db	8f722abd-0123-4494-b71c-a21943484a3c	afee2031-2add-4c5a-b960-f79ac7a80490	2024-10-19 21:55:31.104026	\N	\N	\N	\N	f
beff39ba-9c1f-4ba5-9a75-d9906dbe716d	2	bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	e00c9a01-ea24-48db-ac41-4d39c79f9321	20787148-8572-49d8-b47a-af278f91e43e	2024-10-19 21:55:31.104195	\N	\N	\N	\N	f
bf0ededa-c944-4dc5-9645-bb4b18c05973	1	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.1048	\N	\N	\N	\N	f
bff4959e-5f9f-4204-8625-882d5c615e25	1	39796686-5676-4e48-914a-f405fbead580	d1372bba-be85-473c-8086-02a7c9890140	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:55:31.104719	\N	\N	\N	\N	f
c0a18d0a-73ee-4bf5-aeeb-70a0d428ec75	0	38e635f2-1b68-473b-b941-1e81d253578f	72843603-7dc4-4405-92fa-9a7289ac9b66	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 21:55:31.103679	\N	\N	\N	\N	f
c157d0e1-9e5a-4a6f-9369-9b3b98034737	1	6b0a3307-50cd-4ab8-b239-52ec9227ff19	978e2b3f-9c26-41f0-b3c4-cba2e492767f	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:55:31.106175	\N	\N	\N	\N	f
c18d7f13-dfa7-4261-b21b-8abd68777fae	1	682c7de5-825d-4037-b630-0662381923b7	22e64c46-97c3-40a7-a4aa-4b11eb838446	e00a245f-4a75-4409-bf52-52b890381669	2024-10-19 21:55:31.104765	\N	\N	\N	\N	f
c24be842-8204-46d4-b182-2352cec0b319	0	f6b0f401-8c6e-4d62-a809-15d6006ee100	e21d9b47-d1bb-4c02-9704-acff338cf963	822e7907-b1f2-4062-9070-b8acb5c3b29b	2024-10-19 21:55:31.104871	\N	\N	\N	\N	f
c27ecee5-e28e-4545-bd2f-acb65e19ecc3	0	32a4c31c-5048-43d8-b8e4-5734f0f6741c	84609dec-b050-496e-81be-301a1334919a	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:55:31.104538	\N	\N	\N	\N	f
c28a8584-c8cc-4d3b-8642-9f199e8e4ac9	0	f6b0f401-8c6e-4d62-a809-15d6006ee100	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.105539	\N	\N	\N	\N	f
c30f3617-d55f-4fb2-949e-9c371ff6036a	2	32a4c31c-5048-43d8-b8e4-5734f0f6741c	6e132241-d674-4195-b8c5-b6b4d320e3ce	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 21:55:31.105135	\N	\N	\N	\N	f
c483dcf6-b56d-4233-b414-4b282f236a7d	0	4c97879a-8899-4a52-9e5a-09b6ab8ade5a	6e132241-d674-4195-b8c5-b6b4d320e3ce	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 21:55:31.1044	\N	\N	\N	\N	f
c4c2218c-7320-4dd1-8d84-a19cdd95c88c	2	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	b7594574-0d60-4ffa-b14d-5917c719889b	2024-10-19 21:55:31.106122	\N	\N	\N	\N	f
c52f1017-10f8-40a8-9fb0-9ff2de04cebb	0	2e68a27c-4347-4b83-91a5-a44e7c47473c	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 21:55:31.10289	\N	\N	\N	\N	f
c6187a11-4861-4b8d-9d5f-5bac21d4753d	0	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	a89b63eb-18ed-4f62-8e19-1d977f50a4b2	69914b7d-a41b-43ff-9419-b86ddc8d5cb1	2024-10-19 21:55:31.102815	\N	\N	\N	\N	f
c67b4125-35cf-4e9a-9cb9-fe54beb768e0	1	095f9791-d050-4330-906b-0647ea1786f4	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 21:55:31.104813	\N	\N	\N	\N	f
c8248745-3263-46b1-8e6e-641bb720cb52	2	83ee5b1a-39c3-43e1-81c8-8a31be8ff0d2	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:31.105479	\N	\N	\N	\N	f
c83c8cca-3429-44f4-b761-793ae365c334	0	a5c74b1d-9279-441a-a283-8a9a67e6378c	20105f5a-82e0-4763-b12c-7a5ddc9abf83	d69d03da-d18a-4556-838f-0c9c4d81656d	2024-10-19 21:55:31.104978	\N	\N	\N	\N	f
c969c60c-f47c-4895-9553-0b76610e8670	2	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	e095bbae-68d2-4077-9036-697c526736d4	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:55:31.103167	\N	\N	\N	\N	f
c9bc7878-4354-4073-8ec9-3c63fcb69634	0	26ab939c-c075-43f7-b16e-6a695866d173	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.104009	\N	\N	\N	\N	f
cb1e40b1-81cb-4f5b-aba5-709d88a37761	1	4dfeb3d3-2c9b-497e-8384-55e94215571d	50088da9-86e5-4781-be1e-f8b04a2554d0	906912ce-7b26-4c40-a026-d144fc5c8723	2024-10-19 21:55:31.105355	\N	\N	\N	\N	f
cbc77d22-54ba-4322-993b-cb8609e9ec5a	1	c757d67e-b10d-49f4-b446-3010ae9f9591	7b42cb26-668a-4037-8ffc-68058704a460	a40b73ce-5582-4014-8057-3cf690643a4d	2024-10-19 21:55:31.103909	\N	\N	\N	\N	f
cc808d9f-061d-42eb-879c-d5685aade08b	2	a7c48de9-4698-47cf-ad56-cbaaede24885	07f86036-511f-47d1-b7b7-4543b2eb3303	2e6c3e34-b264-45f0-a9e8-5d3ead7e11ca	2024-10-19 21:55:31.106347	\N	\N	\N	\N	f
ccfb059c-362f-4e7f-ace9-740d6531ba44	1	e33d90bd-73e3-4002-9da2-db6d1180de0e	e095bbae-68d2-4077-9036-697c526736d4	aea921e8-b5c7-4f97-a43e-afd464f25ec3	2024-10-19 21:55:31.106321	\N	\N	\N	\N	f
cdabf09f-c95e-40bc-abe9-f03baeb91125	1	4dfeb3d3-2c9b-497e-8384-55e94215571d	8b92673a-ba81-4629-aea9-41444a46a0dc	bb373eb3-45a7-4f68-8ca5-fb1d24c546d0	2024-10-19 21:55:31.104145	\N	\N	\N	\N	f
cf87af32-470a-46a5-a469-4c21a1f0ef61	1	b3117a33-6280-4bcd-981d-86a87705f58a	14a6b1d0-f886-4f46-9166-a134668d16ab	5636c866-95c5-40c1-9fea-95267dbd8ee9	2024-10-19 21:55:31.105556	\N	\N	\N	\N	f
d0b0f85f-6d03-4e96-a9aa-24dd7e1b77d0	1	26ab939c-c075-43f7-b16e-6a695866d173	bbfef7a3-6fc1-406a-b117-6a2bc70dd418	b43eaefa-d7cf-4efb-a815-c640a3f38f74	2024-10-19 21:55:31.106358	\N	\N	\N	\N	f
d103b735-2a1c-4d99-a2dc-4e6494f4d9d1	0	c11966e2-ee59-4f31-9807-791e2e1c9a3d	e26df0b7-3c68-4669-8b16-e9a9a7f87e3b	b6a3426d-d4da-49e2-b18e-eb40caad3700	2024-10-19 21:55:31.105284	\N	\N	\N	\N	f
d27642af-aaec-42bb-b8b8-b28cac8f4de7	0	2833a7bd-f4a6-472c-9678-883fc2fcda7f	69940db4-d312-4a4e-b7a5-f5bbfd6ddee7	b94655f0-0941-4c62-b692-07ceec473e06	2024-10-19 21:55:31.10313	\N	\N	\N	\N	f
d3952590-efa9-404d-8eff-7a8309d0ff0d	2	77bedc14-cd28-4fd7-8550-90cab9470ba4	35d0da5e-7492-46d3-aaca-17455a353de9	80c16f07-671b-472d-be58-e5fd82bedce0	2024-10-19 21:55:31.10451	\N	\N	\N	\N	f
d46018c4-f7b1-476b-aef8-64495a37347f	0	c757d67e-b10d-49f4-b446-3010ae9f9591	f015b253-2d06-44b2-8f52-1ae49c1a241c	dc15764e-3243-4597-a7ac-b83fb5054d08	2024-10-19 21:55:31.103291	\N	\N	\N	\N	f
d4979567-7e0b-4c1e-a89a-da2350011521	2	c11966e2-ee59-4f31-9807-791e2e1c9a3d	af93b51f-c8b9-4aac-ac95-57bb00c9c3da	b7594574-0d60-4ffa-b14d-5917c719889b	2024-10-19 21:55:31.102981	\N	\N	\N	\N	f
d6eef790-9fd8-4ca3-b696-2b24331e78ca	1	bb4ac43f-0b53-4a37-b12f-aa1a1e32baab	83c97377-4790-4e12-9b61-c0456fe642b2	ca904e4a-c67e-4811-8630-55cbb215c585	2024-10-19 21:55:31.103655	\N	\N	\N	\N	f
d7377c5e-9a3e-423e-a791-1fb164e09cef	1	01b040e9-784f-4edc-a439-2996df603eae	84609dec-b050-496e-81be-301a1334919a	3abaecb3-ccee-4d77-8ca4-559e95866ff6	2024-10-19 21:55:31.105016	\N	\N	\N	\N	f
d79c1b47-e6f5-414d-8976-af088f049b3b	2	77bedc14-cd28-4fd7-8550-90cab9470ba4	78532cb2-f350-4c98-bce2-e94afd8369c6	4bbe97ff-9028-4030-967e-34d7eae8f332	2024-10-19 21:55:31.103449	\N	\N	\N	\N	f
d7a830b0-420e-4f99-b00c-9e380c0def02	1	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	978e2b3f-9c26-41f0-b3c4-cba2e492767f	d220124c-a168-43b3-9668-83b91c086f48	2024-10-19 21:55:31.106222	\N	\N	\N	\N	f
d7c9495f-8f9e-4b72-abdb-9ca3b47d3b8c	1	71691b66-37b0-4a42-95e2-f6d2c14a7d75	5b8a7a2a-53f6-4932-ae19-a3fcdea1b838	f99e97ca-a44a-4433-894f-3af63697fb2f	2024-10-19 21:55:31.10473	\N	\N	\N	\N	f
d7e5b38f-e9a5-4a8b-b0aa-178984d00f7b	0	a7c48de9-4698-47cf-ad56-cbaaede24885	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:31.103041	\N	\N	\N	\N	f
d8dec5a1-3556-4c33-8d81-93a8cadb3279	1	27b1736a-9ead-4a1a-9156-fa8922470eef	14baebc0-0189-423c-a14c-d62ffe981f63	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 21:55:31.104389	\N	\N	\N	\N	f
d8e92e71-7141-49df-815b-a3d02a5e143a	2	4dfeb3d3-2c9b-497e-8384-55e94215571d	1cc85c40-c092-4bee-adeb-3dc17e304563	3d3cb675-d596-49aa-89af-61479d8c8e8d	2024-10-19 21:55:31.10549	\N	\N	\N	\N	f
d93d7111-3fb3-4c8a-93cc-6c421174a8fb	2	f6b0f401-8c6e-4d62-a809-15d6006ee100	72843603-7dc4-4405-92fa-9a7289ac9b66	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 21:55:31.106137	\N	\N	\N	\N	f
da70cb37-a252-4a8d-af1a-e29f09bc39f0	0	82e8961c-1ae8-4912-b4dc-51173b3fdfe6	14baebc0-0189-423c-a14c-d62ffe981f63	f47d785f-5652-45b9-b1ed-9bfddf7807cd	2024-10-19 21:55:31.104076	\N	\N	\N	\N	f
db8f8975-81a6-43a5-b914-a9353067b498	0	deb52d19-077b-42d0-8949-2a7826f2c6a1	b0d1d45b-c71b-4578-8ac0-01c30b49131b	716b8355-1851-445e-b5c9-897643adf03a	2024-10-19 21:55:31.103438	\N	\N	\N	\N	f
dc799b94-665b-4f29-90ca-54a149d81017	2	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:31.103055	\N	\N	\N	\N	f
dd90ed60-4ccc-4c56-9ce7-ffb2f38d9a20	1	6b0a3307-50cd-4ab8-b239-52ec9227ff19	2fa772f8-0fa4-472b-a154-14cf794d50e2	2c230b5e-70ae-4dd0-98ce-503717219fea	2024-10-19 21:55:31.103363	\N	\N	\N	\N	f
de6c843f-d333-4774-9881-1602c402d602	0	295c035f-0873-479b-b155-6746e039e598	3652e96a-9dc0-4f12-817c-1ca7f05807e6	2f7efcc1-14c0-4472-a742-1948dbea238f	2024-10-19 21:55:31.105404	\N	\N	\N	\N	f
dfd2b37f-681d-43fe-aacc-23ea359137d0	2	d3935e8e-8abe-46dc-878a-4434da7af9ec	39ad1877-9e15-4498-88bb-ef6d617a23d2	7f003833-3d8a-4f3c-9c18-7986180847e4	2024-10-19 21:55:31.10369	\N	\N	\N	\N	f
e1462872-49d0-4d1c-bb66-a3c7a7da765d	1	12b52fea-b990-404f-9f77-13d66ec80399	fe1e460d-16ac-4fcd-b512-2413b8cb3256	e79150a4-5947-4f5a-bda6-c9497b32d442	2024-10-19 21:55:31.104464	\N	\N	\N	\N	f
e1f83f3d-67d5-4e62-925a-c00d674e3e18	2	71691b66-37b0-4a42-95e2-f6d2c14a7d75	f18bc355-4a5c-4012-89a6-0044e4bfe36f	8d7eb883-967f-47f7-8fe2-2f898a253886	2024-10-19 21:55:31.104706	\N	\N	\N	\N	f
e22f00dd-da44-49f7-8eb4-04aede135c2b	0	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	eb1b0535-b7f3-430e-b91c-c1feea653f5f	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 21:55:31.10425	\N	\N	\N	\N	f
e3892f36-848a-4d9b-9137-d824c8711e31	1	e33d90bd-73e3-4002-9da2-db6d1180de0e	9612f20e-6fce-4190-bc29-b31d7d3d9188	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:55:31.104261	\N	\N	\N	\N	f
e4151544-e2d6-4e52-afdc-c3d396346a35	1	3581d6ba-8186-4dfd-9e61-e1a2e8f92e3b	4929722e-df51-411e-8c00-59955f7d8fd8	19852718-0f5f-49a9-906e-906e3deda21a	2024-10-19 21:55:31.105942	\N	\N	\N	\N	f
e54a6011-c0b5-44ee-8196-11dc1635c692	1	01b040e9-784f-4edc-a439-2996df603eae	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:55:31.103523	\N	\N	\N	\N	f
e7080728-0bd4-4173-95a0-2d69dd9f88df	1	295c035f-0873-479b-b155-6746e039e598	6e132241-d674-4195-b8c5-b6b4d320e3ce	60f90266-2cae-48bf-9396-e8395980e449	2024-10-19 21:55:31.1062	\N	\N	\N	\N	f
e71bfa90-230a-4a0e-a88c-916aa9ba130b	2	2526b5fd-b6fc-4bd4-a861-5a011bf04db8	6c1fa607-dced-475d-9ad2-1e8ede9071d2	29198ed7-c2be-46cd-a0ed-36bd6a05efbf	2024-10-19 21:55:31.105441	\N	\N	\N	\N	f
e77abb8e-7d73-43d6-aad2-02bdcb6f8680	0	6b0a3307-50cd-4ab8-b239-52ec9227ff19	d1372bba-be85-473c-8086-02a7c9890140	b6a46f96-c234-4a16-9417-cab2d8826b13	2024-10-19 21:55:31.103304	\N	\N	\N	\N	f
e8ca82ef-3f64-4340-aeea-ee0e8687ce65	0	77bedc14-cd28-4fd7-8550-90cab9470ba4	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:55:31.105259	\N	\N	\N	\N	f
ea33eaf9-5b9f-4fc3-824f-a2f357cbe72e	2	01b040e9-784f-4edc-a439-2996df603eae	cb2b279c-19a1-49ef-b47f-bc342a8c7fae	988201d6-d08f-4276-a14e-b4a1e556a53d	2024-10-19 21:55:31.103791	\N	\N	\N	\N	f
ea8698fc-50df-48c9-8b13-12feb0265059	0	39d222f0-3b5a-4b40-9c9f-289ae38d61fa	72843603-7dc4-4405-92fa-9a7289ac9b66	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 21:55:31.104212	\N	\N	\N	\N	f
eb49949b-18fb-4b3b-9496-ffb571880370	2	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	72843603-7dc4-4405-92fa-9a7289ac9b66	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 21:55:31.103031	\N	\N	\N	\N	f
eb6270f1-e19f-4f70-b542-04b971b3159a	1	deb52d19-077b-42d0-8949-2a7826f2c6a1	b6663ea1-57ec-4c3a-9597-da421b3c9484	1adf0cd2-ed45-4722-9875-898a54b06b0b	2024-10-19 21:55:31.103802	\N	\N	\N	\N	f
eb737480-2c84-45be-ba51-e23e9c39ec41	2	6f51d38c-087b-4b4c-a69e-edb5b593a401	72843603-7dc4-4405-92fa-9a7289ac9b66	8ad2ca44-ff48-483b-9606-83fab43d97d8	2024-10-19 21:55:31.106095	\N	\N	\N	\N	f
ebadf603-7661-4890-96aa-a083ad989816	0	01b040e9-784f-4edc-a439-2996df603eae	a36a2bc3-e0e1-43e3-a499-2aec8284e23e	f8ebf6a1-45d5-4b39-a5fa-4c862867ee36	2024-10-19 21:55:31.105591	\N	\N	\N	\N	f
ebcebe80-2389-439a-b18e-a94333f1eaa0	1	f6b0f401-8c6e-4d62-a809-15d6006ee100	950ce7ba-2017-4ab9-bba2-2325f7d35ab6	f7654fc7-97eb-4c4f-a339-3d0fa4590de3	2024-10-19 21:55:31.103996	\N	\N	\N	\N	f
ec492c18-cefb-40b0-99c4-8bc617be0886	0	26ab939c-c075-43f7-b16e-6a695866d173	b55f5bbd-4b95-448a-b38b-a1429002854b	48187f29-f9c6-431d-a0c3-86a6e54abeb4	2024-10-19 21:55:31.103547	\N	\N	\N	\N	f
ed4d4708-cea8-41fe-9df3-3f1d048ce0e6	2	2833a7bd-f4a6-472c-9678-883fc2fcda7f	33725381-a183-49ef-b723-e70495ff6066	4ff132d5-7e7b-4b81-b068-de2f5108f640	2024-10-19 21:55:31.104925	\N	\N	\N	\N	f
ee43958b-3b37-4ab5-aa55-2e76a1754c89	1	054e6a0d-2b3b-4286-b4de-d02ed666ab83	9f64a38d-8cdd-4a21-a529-9747a9331998	bb4ae276-884d-48cb-83fa-8f5b86893088	2024-10-19 21:55:31.105112	\N	\N	\N	\N	f
eee1c2bb-97ca-4f5c-97ff-e08b9c5b28e9	2	e33d90bd-73e3-4002-9da2-db6d1180de0e	0b996fe8-4582-412b-adfb-6fa402c25bf4	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:55:31.105272	\N	\N	\N	\N	f
f13f4f3f-41fe-4aeb-8884-60e3c5bad03a	0	d3935e8e-8abe-46dc-878a-4434da7af9ec	0b996fe8-4582-412b-adfb-6fa402c25bf4	ab6792ec-7e4d-4abf-9c1e-a315d97c422d	2024-10-19 21:55:31.105004	\N	\N	\N	\N	f
f26e5a65-0976-4f32-acf4-4efe4ef37e0d	2	461d4c74-df9d-4c9c-ba94-e33b19a36d0c	d45e1cf5-dfbb-43c4-a614-a6aa2374c588	981b8729-a9e4-40c6-8056-a67972251f6e	2024-10-19 21:55:31.104848	\N	\N	\N	\N	f
f358b940-4a59-4fb5-9fb9-1805352fbd72	1	77bedc14-cd28-4fd7-8550-90cab9470ba4	2b1bcd4d-8082-4ae4-a601-6fab29cc8433	d827cd6e-7c6d-4b7d-b070-20492e078da5	2024-10-19 21:55:31.103067	\N	\N	\N	\N	f
f3aaa37d-d29c-4bf0-b8ad-ba70858c95f4	0	39796686-5676-4e48-914a-f405fbead580	eb1b0535-b7f3-430e-b91c-c1feea653f5f	aceaafa5-c9cb-4369-891a-613943345ca9	2024-10-19 21:55:31.105798	\N	\N	\N	\N	f
f5565693-ed60-4e8d-b5b1-dfe2653ec0f4	2	b6eb7fd3-9d49-4f56-bc0c-53f45d448eb3	705391da-77b5-4f08-b176-301a5f1bbc0d	598ce1ed-f3e9-4bce-9c82-f3ee6b61cc1d	2024-10-19 21:55:31.104157	\N	\N	\N	\N	f
f5871521-6780-48e9-b130-f68c000b54bf	1	c757d67e-b10d-49f4-b446-3010ae9f9591	275ddc93-92b8-419a-ab96-baeb34d89c19	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 21:55:31.10405	\N	\N	\N	\N	f
f5ef00fc-0e9c-4591-8948-fe75e93d9e39	0	682c7de5-825d-4037-b630-0662381923b7	962d9cdb-c2d9-48d4-9187-48db5ddadeb6	0afd67a8-9293-49d6-912a-9e89b50e12fb	2024-10-19 21:55:31.106044	\N	\N	\N	\N	f
f5f9ecca-0ac2-4bcf-a7e0-37994a7dfd7d	1	12b52fea-b990-404f-9f77-13d66ec80399	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:31.103774	\N	\N	\N	\N	f
f6904dc1-30b6-4dc3-91ce-9a3a317515d8	2	d9baa37d-fd31-4cd7-81f3-bcb7902445b0	143437a3-503e-4e95-911d-4c6571ddea8e	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 21:55:31.103945	\N	\N	\N	\N	f
f6b93fdc-7fbc-4b3a-b9e1-788d68b5178a	2	32a4c31c-5048-43d8-b8e4-5734f0f6741c	959b7d55-62bf-42c0-a313-33054551abb5	e6fb00e8-a0ee-460c-bb7d-e33e8189a780	2024-10-19 21:55:31.104449	\N	\N	\N	\N	f
f79062fd-251b-4fc1-bdc6-7c53977040ad	1	4b38441f-00c1-4f4d-b522-3a33eb89ba5c	2eb2ae7e-b05a-45c8-83ef-a23717e17947	bcb42de0-64c2-4e11-890b-7b3de06d0924	2024-10-19 21:55:31.102803	\N	\N	\N	\N	f
f8123b38-7643-4fd3-a68d-2bc77dfda36a	0	095f9791-d050-4330-906b-0647ea1786f4	18e845d8-400b-4d12-a414-9cd440f92677	ecfbb998-043f-40d3-af8c-c0a0cf04f57b	2024-10-19 21:55:31.105722	\N	\N	\N	\N	f
f815a059-3930-477d-a74f-cc8d08c7cacb	1	12b52fea-b990-404f-9f77-13d66ec80399	9612f20e-6fce-4190-bc29-b31d7d3d9188	e641ea43-110f-49b7-b5b2-d115bbfd7168	2024-10-19 21:55:31.104297	\N	\N	\N	\N	f
f9eca8fa-876b-48d2-8087-5a37ae358674	1	095f9791-d050-4330-906b-0647ea1786f4	1f981aae-f40b-4dba-b383-8853d87b23c5	f1a9c58e-5689-4c55-8ec1-54ec35d288bf	2024-10-19 21:55:31.104486	\N	\N	\N	\N	f
fa7fa5df-df55-4f96-85aa-8f4ac3eead43	2	e33d90bd-73e3-4002-9da2-db6d1180de0e	3de591a5-c3e4-4ba1-b148-9973d7a8ac9e	3016ad78-7ee8-4015-85df-d0bb4636f142	2024-10-19 21:55:31.10328	\N	\N	\N	\N	f
fad814b3-7c27-4ce0-9cdb-20f47fbe740d	1	2e68a27c-4347-4b83-91a5-a44e7c47473c	275ddc93-92b8-419a-ab96-baeb34d89c19	fcc71ccd-758e-4034-bf88-b482c5accb65	2024-10-19 21:55:31.103203	\N	\N	\N	\N	f
fba3a6de-14ba-4742-9a5a-ee1811379827	2	eccb8dd8-849f-46c6-88e2-0d636544a8c4	00c05513-4129-4aa6-b79e-983ff13574fc	319e8c19-7e11-481a-bb57-c3c239af2209	2024-10-19 21:55:31.102779	\N	\N	\N	\N	f
fbcd928c-63c8-4f64-9c3e-ca6316be1234	2	eaa3b678-c06c-4ce5-bc56-db6b9ec0a4fd	b3243d6a-7be2-4c83-8a89-dfd4a1976095	d1c01a0d-0e17-4451-9da0-0b4e6579636a	2024-10-19 21:55:31.105245	\N	\N	\N	\N	f
fce79388-0bdc-4e1e-97f4-0062ac1dad3f	2	c11966e2-ee59-4f31-9807-791e2e1c9a3d	49fa1298-7d26-4de1-b197-3005c3d03c0e	88ea9d8d-9bf0-40ed-a794-32835eac461a	2024-10-19 21:55:31.102944	\N	\N	\N	\N	f
fd1e04e1-8ed9-48f4-9a04-931978107d1d	2	f8ff0b49-a5bd-42c2-a557-395bc9216a8a	001b466d-90ee-4f3e-9cea-6f94f7b4c1d5	66f850ce-2d21-43f2-a250-4d4ecdc8f2b0	2024-10-19 21:55:31.103605	\N	\N	\N	\N	f
fd3d2712-f06b-44cd-b375-bb2c8cd15305	2	40e92b5b-736d-4b63-ad95-7dfe2d26bc04	d60eb1e2-73ff-44c0-b1a3-2ecf423bda46	505e9c6b-9476-4fa8-a047-c2e58e6e4399	2024-10-19 21:55:31.103423	\N	\N	\N	\N	f
fdcdafd3-1465-453b-ae58-86f32a4c8a0e	2	5310a3e6-5a16-4090-a632-105cae7d42eb	143437a3-503e-4e95-911d-4c6571ddea8e	374675e8-3e0e-4a90-a8bb-b361657a072e	2024-10-19 21:55:31.105517	\N	\N	\N	\N	f
fe88ae20-d4e8-4fef-a43f-77a447704d9b	1	26ab939c-c075-43f7-b16e-6a695866d173	20105f5a-82e0-4763-b12c-7a5ddc9abf83	d69d03da-d18a-4556-838f-0c9c4d81656d	2024-10-19 21:55:31.105209	\N	\N	\N	\N	f
fe9d006d-41fb-4a93-8e3b-daadf9f44bf7	1	deb52d19-077b-42d0-8949-2a7826f2c6a1	74d9ea46-5729-454f-b994-8faee093ddef	ae722be5-bcc5-4822-b3a0-0a61b8a1f854	2024-10-19 21:55:31.10504	\N	\N	\N	\N	f
ffac07a9-2255-434f-a051-6c02aa4f313f	1	f6b0f401-8c6e-4d62-a809-15d6006ee100	384d21de-6a0f-4c92-b0ef-540ff97079bc	750f454e-4ce5-4cd7-8153-d345999b233b	2024-10-19 21:55:31.105626	\N	\N	\N	\N	f
\.

--
-- TOC entry 2848 (class 2606 OID 16620)
-- Name: groups PK_groups; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups
    ADD CONSTRAINT "PK_groups" PRIMARY KEY (id);


--
-- TOC entry 2851 (class 2606 OID 16625)
-- Name: groups_members PK_groups_members; Type: CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups_members
    ADD CONSTRAINT "PK_groups_members" PRIMARY KEY (id);


--
-- TOC entry 2849 (class 1259 OID 16631)
-- Name: IX_groups_members_group_id_user_profile_id; Type: INDEX; Schema: public; Owner: infinitynetUser
--

CREATE UNIQUE INDEX "IX_groups_members_group_id_user_profile_id" ON public.groups_members USING btree (group_id, user_profile_id);


--
-- TOC entry 2852 (class 2606 OID 16626)
-- Name: groups_members FK_groups_members_groups_group_id; Type: FK CONSTRAINT; Schema: public; Owner: infinitynetUser
--

ALTER TABLE ONLY public.groups_members
    ADD CONSTRAINT "FK_groups_members_groups_group_id" FOREIGN KEY (group_id) REFERENCES public.groups(id) ON DELETE CASCADE;


-- Completed on 2024-10-19 16:01:53 UTC

--
-- PostgreSQL database dump complete
--

